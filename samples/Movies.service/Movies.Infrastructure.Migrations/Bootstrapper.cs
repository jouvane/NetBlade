using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static void AddBootstrapperMigrations(IConfiguration configuration, Action<ILoggingBuilder> configureLogging, string configConnectionString = "ConnectionFluent")
        {
            using ServiceProvider serviceProvider = new ServiceMovieslection().CreateServices(configuration, configureLogging, configConnectionString).BuildServiceProvider(false);
            using IServiceScope scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }

        public static IServiceMovieslection CreateServices(this IServiceMovieslection services, IConfiguration configuration, Action<ILoggingBuilder> configureLogging, string configConnectionString)
        {
            string connectionString = configuration.GetConnectionString(configConnectionString);
            bool inMemory = "InMemory".Equals(connectionString);

            services
                .AddLogging(configureLogging)
                .AddFluentMigratorCore();

            if (inMemory)
            {
                services
                    .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString("Filename=:memory:")
                    .ScanIn(typeof(Bootstrapper).Assembly).For.Migrations());
            }
            else
            {
                services
                    .ConfigureRunner(rb => rb
                    .AddSqlServer2008()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Bootstrapper).Assembly).For.Migrations());
            }

            return services;
        }

        /// <summary>
        /// Down the database
        /// </summary>
        public static void DownDatabase(this IServiceProvider serviceProvider, long version)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateDown(version);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        public static void UpdateDatabase(this IServiceProvider serviceProvider)
        {
            try
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            catch (Exception ex)
            {
                serviceProvider.GetRequiredService<ILogger<IMigrationRunner>>().LogError(ex, "Falha na execução dos Migration.");
            }
        }
    }
}
