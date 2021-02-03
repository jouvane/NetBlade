using NetBlade.CrossCutting.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Movies.Domain;
using Movies.Application;
using Movies.IoC;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using System.Movieslections.Generic;
using System.Reflection;
using NetBlade.Core.Transaction;
using Moq;

namespace Movies.Tests.UnitTest
{
    public class StartupBase : IDisposable
    {
        public StartupBase()
        {
            ServiceMovieslection serviceMovieslection = new ServiceMovieslection();
            this.ConfigureServices(serviceMovieslection);
            this.ServiceProvider = serviceMovieslection.BuildServiceProvider();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ServiceProvider ServiceProvider { get; }

        public void ConfigureServices(IServiceMovieslection services)
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
        public virtual IServiceMovieslection OnConfigureServices(IServiceMovieslection services)
        {
            return services;
        }
    }
}
