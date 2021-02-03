using Microsoft.Extensions.DependencyInjection;

namespace NetBlade.Data.Storage
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddFileSystemStorage(this IServiceCollection services)
        {
            return services
               .AddSingleton<IStorage, FileSystemStorage>();
        }
    }
}
