using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Commands;
using NetBlade.Core.EventContexts;
using NetBlade.Core.Events;
using NetBlade.Core.Mediator;
using NetBlade.Core.Querys;
using NetBlade.Core.Transaction;
using NetBlade.CrossCutting.MediatR.Commands;
using NetBlade.CrossCutting.MediatR.Messages;
using NetBlade.CrossCutting.MediatR.PipelineBehaviors;
using NetBlade.CrossCutting.MediatR.PublishStrategies;
using NetBlade.CrossCutting.MediatR.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetBlade.CrossCutting.MediatR
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCommand<TCommand, TCommandHandler, TCommandResponse>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandHandler : CommandHandlerBase<TCommand, TCommandResponse>
            where TCommandResponse : ICommandResponse
        {
            if (services.All(s => s.ServiceType != typeof(EventContext)))
            {
                services.AddScoped<EventContext>();
            }

            if (services.All(s => s.ServiceType != typeof(TCommand)))
            {
                services.AddTransient<TCommand>();
            }

            if (services.All(s => s.ServiceType != typeof(TCommandHandler)))
            {
                services.AddScoped<TCommandHandler>();
            }

            if (services.All(s => s.ServiceType != typeof(IRequestHandler<CommandMediatR<TCommand>, ICommandResponse>)))
            {
                services
                   .AddScoped(typeof(IRequestHandler<CommandMediatR<TCommand>, ICommandResponse>), x =>
                    {
                        if (typeof(IRequiresNewTransactionScope).IsAssignableFrom(typeof(TCommand)))
                        {
                            return new CommandHandlerMediatR<TCommand, TCommandResponse>(x.GetService<IServiceScopeFactory>(), typeof(TCommandHandler));
                        }
                        else
                        {
                            CommandHandlerBase<TCommand, TCommandResponse> commandHandler = x.GetService<TCommandHandler>();
                            return new CommandHandlerMediatR<TCommand, TCommandResponse>(commandHandler);
                        }
                    });
            }

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services, Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Any())
            {
                MethodInfo method = typeof(Bootstrapper)
                   .GetMethod(nameof(Bootstrapper.AddCommand), BindingFlags.Static | BindingFlags.NonPublic);

                IEnumerable<Type> commandsTypes = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(typeof(Command))));
                IEnumerable<Type> commandResponseTypes = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(typeof(CommandResponse))));
                List<Type> commandHandlerBaseTypes = new List<Type>();

                foreach (Type command in commandsTypes)
                {
                    foreach (Type commandResponseType in commandResponseTypes)
                    {
                        commandHandlerBaseTypes.Add(typeof(CommandHandlerBase<,>).MakeGenericType(command, commandResponseType));
                    }

                    Type type = commandHandlerBaseTypes.FirstOrDefault(f => f.IsAssignableFrom(null));

                    Type[] concreteTypes = null; //assembly.GetTypes().Where(t => type.IsAssignableFrom(t));

                    foreach (Type concreteType in concreteTypes)
                    {
                        MethodInfo genericMethod = method.MakeGenericMethod(command, concreteType);
                        genericMethod.Invoke(null, new[] { services });
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddEvent<TMessage, TEventHandler>(this IServiceCollection services)
            where TMessage : Message
            where TEventHandler : class, IEventHandler<TMessage>
        {
            if (services.All(s => s.ServiceType != typeof(EventContext)))
            {
                services.AddScoped<EventContext>();
            }

            if (services.All(s => s.ServiceType != typeof(TEventHandler)))
            {
                services.AddScoped<TEventHandler>();
            }

            services
               .AddScoped(typeof(INotificationHandler<MessageMediatR<TMessage>>), x =>
                {
                    if (typeof(IRequiresNewTransactionScope).IsAssignableFrom(typeof(TMessage)))
                    {
                        return new MessageHandlerMediatR<TMessage>(x.GetService<IServiceScopeFactory>(), typeof(TEventHandler));
                    }
                    else
                    {
                        IEventHandler<TMessage> messageHandler = x.GetService<TEventHandler>();
                        return new MessageHandlerMediatR<TMessage>(messageHandler);
                    }
                });

            return services;
        }

        public static IServiceCollection AddEvents(this IServiceCollection services, Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Any())
            {
                MethodInfo method = typeof(Bootstrapper)
                   .GetMethod(nameof(Bootstrapper.AddEvent), BindingFlags.Static | BindingFlags.Public);

                IEnumerable<Type> events = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(typeof(Message))));
                if (events.Any())
                {
                    foreach (Assembly assemblie in assemblies)
                    {
                        foreach (Type @event in events)
                        {
                            Type type = typeof(IEventHandler<>).MakeGenericType(@event);
                            IEnumerable<Type> concreteTypes = assemblie.GetTypes().Where(t => type.IsAssignableFrom(t));

                            foreach (Type concreteType in concreteTypes)
                            {
                                if (concreteType != null)
                                {
                                    MethodInfo genericMethod = method.MakeGenericMethod(@event, concreteType);
                                    genericMethod.Invoke(null, new[] { services });
                                }
                            }
                        }
                    }
                }
            }

            return services;
        }

        public static IServiceCollection AddMediatoInMemory(this IServiceCollection services, Action<MediatRServiceConfiguration> configuration, Assembly[] assemblies = null, bool autoAddEvents = true, bool autoAddCommands = true, bool autoAddQuerys = true)
        {
            services.AddMediatR(configuration, typeof(int));

            services.AddScoped<Publisher>();
            services.AddScoped<IMediatorHandler, MediatoInMemoryHandler>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineBehavior<,>));

            if (assemblies != null && assemblies.Length > 0)
            {
                if (autoAddEvents)
                {
                    //services.AddEvents(assemblies);
                }

                if (autoAddCommands)
                {
                    //services.AddCommands(assemblies);
                }

                if (autoAddQuerys)
                {
                    //services.AddQuerys(assemblies);
                }
            }

            return services;
        }

        public static IServiceCollection AddQuery<TQuery, TQueryHandler, TQueryResponse>(this IServiceCollection services)
            where TQuery : class, IQuery
            where TQueryHandler : QueryHandlerBase<TQuery, TQueryResponse>
            where TQueryResponse : IQueryResponse
        {
            if (services.All(s => s.ServiceType != typeof(EventContext)))
            {
                services.AddScoped<EventContext>();
            }

            if (services.All(s => s.ServiceType != typeof(TQuery)))
            {
                services.AddTransient<TQuery>();
            }

            if (services.All(s => s.ServiceType != typeof(TQueryHandler)))
            {
                services.AddScoped<TQueryHandler>();
            }

            if (services.All(s => s.ServiceType != typeof(IRequestHandler<QueryMediatR<TQuery>, IQueryResponse>)))
            {
                services.AddScoped(typeof(IRequestHandler<QueryMediatR<TQuery>, IQueryResponse>), x =>
                {
                    if (typeof(IRequiresNewTransactionScope).IsAssignableFrom(typeof(TQuery)))
                    {
                        return new QueryHandlerMediatR<TQuery, TQueryResponse>(x.GetService<IServiceScopeFactory>(), typeof(TQueryHandler));
                    }
                    else
                    {
                        QueryHandlerBase<TQuery, TQueryResponse> queryHandler = x.GetService<TQueryHandler>();
                        return new QueryHandlerMediatR<TQuery, TQueryResponse>(queryHandler);
                    }
                });
            }

            return services;
        }

        public static IServiceCollection AddQuerys(this IServiceCollection services, Assembly[] assemblies)
        {
            if (assemblies != null && assemblies.Any())
            {
                MethodInfo method = typeof(Bootstrapper)
                   .GetMethod(nameof(Bootstrapper.AddQuery), BindingFlags.Static | BindingFlags.NonPublic);

                IEnumerable<Type> querys = assemblies.SelectMany(s => s.GetTypes().Where(t => t.IsSubclassOf(typeof(Query))));

                foreach (Type query in querys)
                {
                    Type type = typeof(QueryHandlerBase<,>).MakeGenericType(query);
                    IEnumerable<Type> concreteTypes = typeof(Bootstrapper).Assembly.GetTypes().Where(t => type.IsAssignableFrom(t));

                    foreach (Type concreteType in concreteTypes)
                    {
                        MethodInfo genericMethod = method.MakeGenericMethod(query, concreteType);
                        genericMethod.Invoke(null, new[] { services });
                    }
                }
            }

            return services;
        }
    }
}
