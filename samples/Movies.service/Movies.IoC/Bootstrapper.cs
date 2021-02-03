using AutoMapper;
using FluentValidation;
using Inmetro.CrossCutting.MediatR;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application;
using Movies.Application.Abstraction;
using Movies.CrossCutting;
using Movies.Domain;
using Movies.Infrastructure.Repository;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Movies.IoC
{
    public static class Bootstrapper
    {
        public static Assembly[] Assemblies =
        {
            Assembly.GetEntryAssembly(),
            typeof(Application.Bootstrapper).Assembly,
            typeof(Application.Abstraction.Bootstrapper).Assembly,
            typeof(Movies.CrossCutting.Bootstrapper).Assembly,
            typeof(Domain.Bootstrapper).Assembly,
            typeof(Infrastructure.Repository.Bootstrapper).Assembly
        };

        [ExcludeFromCodeCoverage]
        public static IServiceMovieslection AddBootstrapperIoC(this IServiceMovieslection services, IConfiguration configuration)
        {
            services.AddBootstrapperApplication()
               .AddBootstrapperApplicationAbstraction()
               .AddBootstrapperCrossCutting(configuration)
               .AddBootstrapperDomain()
               .AddBootstrapperRepository(configuration);

            services
               .AddMediatoInMemory(Bootstrapper.ConfigMediatoInMemory);

            services.AddValidations();

            services
               .AddAutoMapper(Bootstrapper.Assemblies, ServiceLifetime.Transient);

            return services;
        }

        public static IServiceMovieslection AddValidations(this IServiceMovieslection services)
        {
            AssemblyScanner.FindValidatorsInAssemblies(Bootstrapper.Assemblies)
               .ForEach(f =>
                {
                    services.AddTransient(f.InterfaceType, f.ValidatorType);
                    services.AddTransient(f.ValidatorType, f.ValidatorType);
                });

            return services;
        }

        public static void ConfigMediatoInMemory(MediatRServiceConfiguration config)
        {
            config.AsScoped();
        }
    }
}
