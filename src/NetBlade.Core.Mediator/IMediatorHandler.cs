using NetBlade.Core.Commands;
using NetBlade.Core.Events;
using NetBlade.Core.Querys;
using System.Threading.Tasks;

namespace NetBlade.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task RaiseEvent<T>(T @event)
            where T : Event;

        Task<CommandResponse<TData>> SendCommand<TData>(ICommand command);

        Task<ICommandResponse> SendCommand<TCommand>(TCommand command)
            where TCommand : class, ICommand;

        Task<CommandResponse<TData>> SendCommand<TCommand, TData>(TCommand command)
            where TCommand : class, ICommand;

        Task<QueryResponse<TData>> SendQuery<TData>(IQuery query);

        Task<QueryResponse<TData>> SendQuery<TQuery, TData>(TQuery query)
            where TQuery : class, IQuery;

        Task<IQueryResponse> SendQuery<TQuery>(TQuery query)
            where TQuery : class, IQuery;
    }
}
