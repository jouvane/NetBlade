using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Movies.Application;
using Movies.Domain;
using Movies.IoC;
using NetBlade.Core.Transaction;
using NetBlade.CrossCutting.MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Movies.Tests.UnitTest
{
    public class StartupBase : IDisposable
    {
        public StartupBase()
        {
            ServiceCollection serviceMovieslection = new ServiceCollection();
            this.ConfigureServices(serviceMovieslection);
            this.ServiceProvider = serviceMovieslection.BuildServiceProvider();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ServiceProvider ServiceProvider { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);
            this.Configuration = builder.Build();
            services.AddSingleton(this.Configuration);

            List<Assembly> ass = new List<Assembly>(IoC.Bootstrapper.Assemblies);
            ass.Add(typeof(Movies.API.Startup).Assembly);
            IoC.Bootstrapper.Assemblies = ass.ToArray();

            services.AddLogging(l => l.AddSerilog(dispose: true));
            Log.Logger = new LoggerConfiguration()
               .ReadFrom
               .Configuration(this.Configuration)
               .CreateLogger();

            services
                .AddMediatoInMemory(IoC.Bootstrapper.ConfigMediatoInMemory)
                .AddBootstrapperApplication()
                .AddBootstrapperDomain()
                .AddAutoMapper(IoC.Bootstrapper.Assemblies, ServiceLifetime.Transient);

            services
                .AddValidations();

            services
                .AddScoped(_ => Mock.Of<ITransactionManager>());

            _ = this.OnConfigureServices(services);
        }

        public void Dispose()
        {
            this.ServiceProvider.Dispose();
        }

        [ExcludeFromCodeCoverage]
        public virtual IServiceCollection OnConfigureServices(IServiceCollection services)
        {
            return services;
        }
    }
}
