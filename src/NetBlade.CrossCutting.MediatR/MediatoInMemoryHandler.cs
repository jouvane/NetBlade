using MediatR;
using NetBlade.Core.Commands;
using NetBlade.Core.Events;
using NetBlade.Core.Mediator;
using NetBlade.Core.Querys;
using NetBlade.CrossCutting.MediatR.Commands;
using NetBlade.CrossCutting.MediatR.Messages;
using NetBlade.CrossCutting.MediatR.PublishStrategies;
using NetBlade.CrossCutting.MediatR.Querys;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR
{
    public class MediatoInMemoryHandler : IMediatorHandler
    {
        protected readonly IMediator _mediator;
        protected readonly Publisher _publisher;

        private static readonly MethodInfo _internalPublishMethodInfo = typeof(MediatoInMemoryHandler).GetMethod(nameof(MediatoInMemoryHandler.InternalPublish), BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo _internalSendCommandMethodInfo = typeof(MediatoInMemoryHandler).GetMethod(nameof(MediatoInMemoryHandler.InternalSendCommand), BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo _internalSendQueryMethodInfo = typeof(MediatoInMemoryHandler).GetMethod(nameof(MediatoInMemoryHandler.InternalSendQuery), BindingFlags.Instance | BindingFlags.NonPublic);

        public MediatoInMemoryHandler(IMediator mediator, Publisher publisher)
        {
            this._mediator = mediator;
            this._publisher = publisher;
        }

        public async Task RaiseEvent<T>(T @event)
            where T : Event
        {
            if (@event != null)
            {
                @event.Raised = true;
                if (typeof(T) != @event.GetType())
                {
                    await (Task)MediatoInMemoryHandler._internalPublishMethodInfo.MakeGenericMethod(@event.GetType()).Invoke(this, new object[] { @event });
                }
                else
                {
                    await this.InternalPublish(@event);
                }
            }
        }

        public async Task<CommandResponse<TData>> SendCommand<TData>(ICommand command)
        {
            ICommandResponse result = await this.SendCommand(command);
            return (CommandResponse<TData>)result;
        }

        public async Task<CommandResponse<TData>> SendCommand<TCommand, TData>(TCommand command)
            where TCommand : class, ICommand
        {
            ICommandResponse result = await this.SendCommand(command);
            return (CommandResponse<TData>)result;
        }

        public async Task<ICommandResponse> SendCommand<TCommand>(TCommand command)
           where TCommand : class, ICommand
        {
            ICommandResponse result;
            if (typeof(TCommand) != command.GetType())
            {
                result = await (Task<ICommandResponse>)MediatoInMemoryHandler._internalSendCommandMethodInfo.MakeGenericMethod(command.GetType()).Invoke(this, new object[] { command });
            }
            else
            {
                result = await this.InternalSendCommand<TCommand>(command);
            }

            return result;
        }

        public async Task<QueryResponse<TData>> SendQuery<TQuery, TData>(TQuery query)
            where TQuery : class, IQuery
        {
            IQueryResponse result = await this.SendQuery<TQuery>(query);
            return (QueryResponse<TData>)result;
        }

        public async Task<IQueryResponse> SendQuery<TQuery>(TQuery query)
            where TQuery : class, IQuery
        {
            IQueryResponse result;
            if (typeof(TQuery) != query.GetType())
            {
                result = await (Task<IQueryResponse>)MediatoInMemoryHandler._internalSendQueryMethodInfo.MakeGenericMethod(query.GetType()).Invoke(this, new object[] { query });
            }
            else
            {
                result = await this.InternalSendQuery<TQuery>(query);
            }

            return result;
        }

        public async Task<QueryResponse<TData>> SendQuery<TData>(IQuery query)
        {
            IQueryResponse result = await this.SendQuery(query);
            return (QueryResponse<TData>)result;
        }

        private async Task InternalPublish<T>(T message)
            where T : Message
        {
            await this._publisher.Publish(new MessageMediatR<T>(message), CancellationToken.None);
        }

        private async Task<ICommandResponse> InternalSendCommand<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            ICommandResponse result = await this._mediator.Send(new CommandMediatR<TCommand>(command));
            return result;
        }

        private async Task<IQueryResponse> InternalSendQuery<TQuery>(TQuery query)
            where TQuery : class, IQuery
        {
            IQueryResponse result = await this._mediator.Send(new QueryMediatR<TQuery>(query));
            return result;
        }
    }
}
