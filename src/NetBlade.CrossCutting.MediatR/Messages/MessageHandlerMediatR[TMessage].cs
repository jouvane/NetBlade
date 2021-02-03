using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.Messages
{
    internal class MessageHandlerMediatR<TMessage> : INotificationHandler<MessageMediatR<TMessage>>, IDisposable
        where TMessage : Message
    {
        private static readonly object _lock = new object();
        private readonly IEventHandler<TMessage> _messageHandler;
        private readonly IServiceScope _serviceScope;
        private bool _disposed;

        internal MessageHandlerMediatR(IServiceScopeFactory serviceScopeFactory, Type typeMessageHandler)
        {
            this._serviceScope = serviceScopeFactory.CreateScope();
            this._messageHandler = (IEventHandler<TMessage>)this._serviceScope.ServiceProvider.GetService(typeMessageHandler);
        }

        internal MessageHandlerMediatR(IEventHandler<TMessage> messageHandler)
        {
            this._messageHandler = messageHandler;
        }

        public void Dispose()
        {
            if (this._serviceScope != null)
            {
                bool disposed;
                lock (MessageHandlerMediatR<TMessage>._lock)
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

        public async Task Handle(MessageMediatR<TMessage> notification, CancellationToken cancellationToken)
        {
            await this._messageHandler.Handle(notification.BaseMessage, cancellationToken);
        }
    }
}