using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Transaction;

namespace NetBlade.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddTransactionManager(this IServiceCollection services)
        {
            return services
               .AddScoped<ITransactionManager, TransactionManager>();
        }
    }
}
