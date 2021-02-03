using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.PublishStrategies
{
    public class CustomMediator : Mediator
    {
        private readonly Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> _publish;

        public CustomMediator(ServiceFactory serviceFactory, Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> publish)
            : base(serviceFactory)
        {
            this._publish = publish;
        }

        protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
        {
            return this._publish(allHandlers, notification, cancellationToken);
        }
    }
}
