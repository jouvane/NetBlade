using Inmetro.Core.Commands;
using Inmetro.Core.Events;
using Inmetro.Core.Querys;
using Inmetro.CrossCutting.MediatR;
using Inmetro.Data.Pagination;
using Microsoft.Extensions.DependencyInjection;
using System.Movieslections.Generic;

namespace Movies.Application
{
    public static class Bootstrapper
    {
        public static IServiceMovieslection AddBootstrapperApplication(this IServiceMovieslection services)
        {
            //services
               //.AddCommand<Command, CommandHandler, CommandResponse>()
                ;

            //services
               //.AddQuery<Query, QueryHandler, QueryResponse<DataTransferObjectOut>>()
               ;

         
            //services
                //.AddEvent<DomainEvent, EventHandler>()
                ;

            return services;
        }
    }
}
