using MediatR;
using NetBlade.Core.Events;
using NetBlade.Core.Events.PublishStrategy;
using NetBlade.CrossCutting.MediatR.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.PublishStrategies
{
    public class Publisher
    {
        private readonly ServiceFactory _serviceFactory;
        public IDictionary<Type, IMediator> PublishStrategies = new Dictionary<Type, IMediator>();

        public Publisher(ServiceFactory serviceFactory)
        {
            this._serviceFactory = serviceFactory;

            this.PublishStrategies[typeof(IAsync)] = new CustomMediator(this._serviceFactory, this.AsyncContinueOnException);
            this.PublishStrategies[typeof(IParallelNoWait)] = new CustomMediator(this._serviceFactory, this.ParallelNoWait);
            this.PublishStrategies[typeof(IParallelWhenAll)] = new CustomMediator(this._serviceFactory, this.ParallelWhenAll);
            this.PublishStrategies[typeof(IParallelWaitAll)] = new CustomMediator(this._serviceFactory, this.ParallelWaitAll);
            this.PublishStrategies[typeof(IParallelWhenAny)] = new CustomMediator(this._serviceFactory, this.ParallelWhenAny);
            this.PublishStrategies[typeof(ISyncContinueOnException)] = new CustomMediator(this._serviceFactory, this.SyncContinueOnException);
            this.PublishStrategies[typeof(ISyncStopOnException)] = new CustomMediator(this._serviceFactory, this.SyncStopOnException);
        }

        public virtual Type DefaultStrategy
        {
            get => typeof(IParallelWaitAll);
        }

        public virtual async Task AsyncContinueOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            List<Exception> exceptions = new List<Exception>();

            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                try
                {
                    tasks.Add(handler(notification, cancellationToken));
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public virtual Task ParallelNoWait(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                Task.Run(() => handler(notification, cancellationToken), cancellationToken);
            }

            return Task.CompletedTask;
        }

        public virtual Task ParallelWaitAll(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();

            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken), cancellationToken));
            }

            Task.WaitAll(tasks.ToArray(), cancellationToken);
            return Task.CompletedTask;
        }

        public virtual Task ParallelWhenAll(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();

            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken), cancellationToken));
            }

            return Task.WhenAll(tasks);
        }

        public virtual Task ParallelWhenAny(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();

            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                tasks.Add(Task.Run(() => handler(notification, cancellationToken), cancellationToken));
            }

            return Task.WhenAny(tasks);
        }

        public virtual Task Publish<TMessage>(MessageMediatR<TMessage> notification, CancellationToken cancellationToken)
            where TMessage : Message
        {
            IMediator mediator;
            switch (notification.BaseMessage)
            {
                case IAsync _:
                    mediator = this.PublishStrategies[typeof(IAsync)];
                    break;

                case IParallelNoWait _:
                    mediator = this.PublishStrategies[typeof(IParallelNoWait)];
                    break;

                case IParallelWhenAll _:
                    mediator = this.PublishStrategies[typeof(IParallelWhenAll)];
                    break;

                case IParallelWhenAny _:
                    mediator = this.PublishStrategies[typeof(IParallelWhenAny)];
                    break;

                case ISyncContinueOnException _:
                    mediator = this.PublishStrategies[typeof(ISyncContinueOnException)];
                    break;

                case ISyncStopOnException _:
                    mediator = this.PublishStrategies[typeof(ISyncStopOnException)];
                    break;

                default:
                    mediator = this.PublishStrategies[this.DefaultStrategy];
                    break;
            }

            return mediator.Publish(notification, cancellationToken);
        }

        public virtual async Task SyncContinueOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                try
                {
                    await handler(notification, cancellationToken).ConfigureAwait(false);
                }
                catch (AggregateException ex)
                {
                    exceptions.AddRange(ex.Flatten().InnerExceptions);
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public virtual async Task SyncStopOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            foreach (Func<INotification, CancellationToken, Task> handler in handlers)
            {
                await handler(notification, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
