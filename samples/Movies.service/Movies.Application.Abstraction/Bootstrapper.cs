using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Application.Abstraction
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceCollection AddBootstrapperApplicationAbstraction(this IServiceCollection services)
        {
            return services;
        }
    }
}
