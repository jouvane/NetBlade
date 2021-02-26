using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetBlade.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static void AddBootstrapperMigrations(IConfiguration configuration, Action<ILoggingBuilder> configureLogging, string configConnectionString = "ConnectionFluent")
        {
            using ServiceProvider serviceProvider = new ServiceCollection().CreateServices(configuration, configureLogging, configConnectionString).BuildServiceProvider(false);
            using IServiceScope scope = serviceProvider.CreateScope();
            Bootstrapper.UpdateDatabase(scope.ServiceProvider);
        }

        public static void CreateDatabase(string connectionStr)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionStr);
            string dataBase = sqlConnectionStringBuilder.InitialCatalog;
            sqlConnectionStringBuilder.Remove("Initial Catalog");

            using ConnectionManager connectionManager = new ConnectionManager(new SqlConnection(sqlConnectionStringBuilder.ConnectionString));
            using IDbConnection conn = connectionManager.Open();
            using IDbCommand cmd = conn.CreateCommand();

            cmd.CommandText = $@"
            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dataBase}')
            BEGIN
                CREATE DATABASE {dataBase}
            END";

            _ = cmd.ExecuteScalar();
        }

        public static IServiceCollection CreateServices(this IServiceCollection services, IConfiguration configuration, Action<ILoggingBuilder> configureLogging, string configConnectionString)
        {
            string connectionString = configuration.GetConnectionString(configConnectionString);
            bool inMemory = "InMemory".Equals(connectionString);

            services
                .AddLogging(configureLogging)
                .AddFluentMigratorCore();

            if (!inMemory)
            {
                services
                    .ConfigureRunner(rb => rb
                    .AddSqlServer2008()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Bootstrapper).Assembly).For.Migrations());

                Bootstrapper.CreateDatabase(connectionString);
            }

            return services;
        }

        /// <summary>
        /// Down the database
        /// </summary>
        public static void DownDatabase(this IServiceProvider serviceProvider, long version)
        {
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateDown(version);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        public static void UpdateDatabase(this IServiceProvider serviceProvider)
        {
            try
            {
                IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            catch (Exception ex)
            {
                serviceProvider.GetRequiredService<ILogger<IMigrationRunner>>().LogError(ex, "Falha na execução dos Migration.");
            }
        }
    }
}
