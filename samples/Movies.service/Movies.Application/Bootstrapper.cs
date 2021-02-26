using Microsoft.Extensions.DependencyInjection;

namespace Movies.Application
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddBootstrapperApplication(this IServiceCollection services)
        {
            //services
            //.AddCommand<Command, CommandHandler, CommandResponse>();

            //services
            //.AddQuery<Query, QueryHandler, QueryResponse<DataTransferObjectOut>>();


            //services
            //.AddEvent<DomainEvent, EventHandler>();

            return services;
        }
    }
}
