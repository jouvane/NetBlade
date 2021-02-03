using NetBlade.Core.Events;
using NetBlade.Core.Mediator;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBlade.Core.EventContexts
{
    public class EventContext
    {
        internal readonly ConcurrentDictionary<string, Event> _events;

        private readonly object _lockObj = new object();
        private readonly IMediatorHandler _mediatorHandler;

        public EventContext(IMediatorHandler mediatorHandler)
        {
            this._mediatorHandler = mediatorHandler;
            this._events = new ConcurrentDictionary<string, Event>();
        }

        public bool AutoRaise { get; internal set; }

        public virtual async Task RaiseEvents()
        {
            List<Event> events = new List<Event>();
            if (this._events.Any())
            {
                lock (this._lockObj)
                {
                    events.AddRange(this._events.Select(s => s.Value).ToList());
                    this._events.Clear();
                }
            }

            await this.RaiseEvents(events);
        }

        public virtual Task RaiseEvents(IEnumerable<Event> events)
        {
            Task[] tasks = events
                .Where(e => !e.Raised)
               .Select(async @event =>
               {
                   await this._mediatorHandler.RaiseEvent(@event);
                   @event.Raised = true;
               })
               .ToArray();

            if (tasks.Any())
            {
                Task.WaitAll(tasks);
            }

            return Task.CompletedTask;
        }

        public virtual async Task RegisterEvents(params Event[] events)
        {
            if (events != null && events.Any())
            {
                List<Event> eventsAutoRaise = new List<Event>();
                lock (this._lockObj)
                {
                    foreach (Event @event in events)
                    {
                        if (this.EventIsAutoRaise(@event))
                        {
                            eventsAutoRaise.Add(@event);
                        }
                        else
                        {
                            this._events.TryAdd(@event.IdEvent.ToString(), @event);
                        }
                    }
                }

                await this.RaiseEvents(eventsAutoRaise);
            }
        }
    }
}
