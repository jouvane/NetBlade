using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Transaction;

namespace NetBlade.Data.EF
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddGenericRepositoryEF(this IServiceCollection services)
        {
            return services
               .AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        }

        public static IServiceCollection AddTransactionManagerEF(this IServiceCollection services)
        {
            return services
               .AddScoped<ITransactionManager, TransactionManagerEF>();
        }
    }
}
