using Microsoft.Extensions.DependencyInjection;

namespace Movies.Domain
{
    public static class Bootstrapper
    {
        public static IServiceMovieslection AddBootstrapperDomain(this IServiceMovieslection services)
        {
            return services
              //.AddScoped<Service>()
              ;
        }
    }
}
