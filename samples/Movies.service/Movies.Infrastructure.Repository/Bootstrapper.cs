using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Movies.Infrastructure.Repository.Context;
using NetBlade.Data;
using NetBlade.Data.EF;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Infrastructure.Repository
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceCollection AddBootstrapperRepository(this IServiceCollection services, IConfiguration configuration, string configConnectionString = "Connection", string sectionRepositoryBaseOption = "NetBlade:Data.EF:RepositoryBaseOption")
        {
            string connectionString = configuration.GetConnectionString(configConnectionString);
            bool inMemory = "InMemory".Equals(connectionString);
            if (inMemory)
            {
                services
                     .AddScoped<IConnectionManager, ConnectionManager>(s => new ConnectionManager(new SqlConnection()))
                     .AddDbContext<MoviesDbContext>((s, options) =>
                     {
                         options
                             .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>())
                             .UseInMemoryDatabase(nameof(MoviesDbContext), (InMemoryDbContextOptionsBuilder opt) => { })
                             .EnableSensitiveDataLogging();
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
                         options
                             .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>())
                             .UseSqlServer(dbConnection as DbConnection ?? throw new InvalidOperationException(), (SqlServerDbContextOptionsBuilder opt) => { })
                             .EnableSensitiveDataLogging();
                     });
            }

            services
                .AddTransactionManagerEF()
                .AddGenericRepositoryEF()
                .AddScoped<DbContext>(s => s.GetService<MoviesDbContext>());

            //services
            //.AddScoped<IRepository, Repository>();

            return services;
        }
    }
}
