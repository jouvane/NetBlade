using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Movies.CrossCutting
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceCollection AddBootstrapperCrossCutting(this IServiceCollection services, IConfiguration configuration)
        {
            _ = configuration;
            return services;
        }

    }
}
