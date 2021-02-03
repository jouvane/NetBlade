using NetBlade.Data;
using NetBlade.Data.EF;
using NetBlade.Data.EF.ConfigureOptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Infrastructure.Repository.Context;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.Sqlite;

namespace Movies.Infrastructure.Repository
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceMovieslection AddBootstrapperRepository(this IServiceMovieslection services, IConfiguration configuration, string configConnectionString = "Connection", string sectionRepositoryBaseOption = "NetBlade:Data.EF:RepositoryBaseOption")
        {
            string connectionString = configuration.GetConnectionString(configConnectionString);
            bool inMemory = "InMemory".Equals(connectionString);
            if (inMemory)
            {
                services
                    .AddScoped<IConnectionManager, ConnectionManager>(s => new ConnectionManager(new SqliteConnection("Filename=:memory:")))
                    .AddDbContext<MoviesDbContext>((s, options) =>
                    {
                        options.UseSqlite(Bootstrapper.GetDbConnection(s), (SqliteDbContextOptionsBuilder sqliteOptionsAction) => { });
                    });
            }
            else
            {
                services
                    .AddScoped<IConnectionManager, ConnectionManager>(s => new ConnectionManager(new SqlConnection(connectionString)))
                    .AddDbContext<MoviesDbContext>((s, options) =>
                    {
                        IConnectionManager connectionManager = s.GetService<IConnectionManager>();
                        IDbConnection dbConnection = connectionManager.Get();
                        options.UseSqlServer(Bootstrapper.GetDbConnection(s), (SqlServerDbContextOptionsBuilder opt) => { });
                    });
            }

            services
                .AddTransactionManagerEF()
                .AddGenericRepositoryEF()
                .AddScoped<DbContext>(s => s.GetService<MoviesDbContext>())
                .Configure<RepositoryBaseOption>(configuration.GetSection(sectionRepositoryBaseOption));

            //services
            //.AddScoped<IRepository, Repository>()
            ;

            return services;
        }

        private static DbConnection GetDbConnection(IServiceProvider serviceProvider)
        {
            IConnectionManager connectionManager = serviceProvider.GetService<IConnectionManager>();
            IDbConnection dbConnection = connectionManager.Get();

            return dbConnection as DbConnection ?? throw new InvalidOperationException();
        }
    }
}
