using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Movies.CrossCutting
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceMovieslection AddBootstrapperCrossCutting(this IServiceMovieslection services, IConfiguration configuration)
        {
            _ = configuration;
            return services;
        }

    }
}
