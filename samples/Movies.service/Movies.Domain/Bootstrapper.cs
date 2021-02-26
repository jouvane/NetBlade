using Microsoft.Extensions.DependencyInjection;

namespace Movies.Domain
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddBootstrapperDomain(this IServiceCollection services)
        {
            return services
              //.AddScoped<Service>()
              ;
        }
    }
}
