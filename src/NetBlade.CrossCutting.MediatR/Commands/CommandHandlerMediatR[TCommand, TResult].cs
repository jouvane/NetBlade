using NetBlade.Core.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.Commands
{
    internal class CommandHandlerMediatR<TCommand, TCommandResponse> : IRequestHandler<CommandMediatR<TCommand>, ICommandResponse>, IDisposable
        where TCommand : class, ICommand
        where TCommandResponse : ICommandResponse
    {
        private static readonly object _lock = new object();
        private readonly CommandHandlerBase<TCommand, TCommandResponse> _commandHandler;
        private readonly IServiceScope _serviceScope;
        private bool _disposed;

        internal CommandHandlerMediatR(IServiceScopeFactory serviceScopeFactory, Type typeCommandHandler)
        {
            this._serviceScope = serviceScopeFactory.CreateScope();
            this._commandHandler = (CommandHandlerBase<TCommand, TCommandResponse>)this._serviceScope.ServiceProvider.GetService(typeCommandHandler);
        }

        internal CommandHandlerMediatR(CommandHandlerBase<TCommand, TCommandResponse> commandHandler)
        {
            this._commandHandler = commandHandler;
        }

        public void Dispose()
        {
            if (this._serviceScope != null)
            {
                bool disposed;
                lock (CommandHandlerMediatR<Command, ICommandResponse>._lock)
                {
                    disposed = this._disposed;
                    this._disposed = true;
                }

                if (!disposed)
                {
                    this._serviceScope.Dispose();
                }
            }
        }

        public async Task<ICommandResponse> Handle(CommandMediatR<TCommand> request, CancellationToken cancellationToken)
        {
            return await this._commandHandler.Handle(request.BaseCommand, cancellationToken);
        }
    }
}