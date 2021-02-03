using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Application.Abstraction
{
    [ExcludeFromCodeCoverage]
    public static class Bootstrapper
    {
        public static IServiceMovieslection AddBootstrapperApplicationAbstraction(this IServiceMovieslection services)
        {
            return services;
        }
    }
}
