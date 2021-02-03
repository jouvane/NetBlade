using NetBlade.Core.Domain;
using NetBlade.Core.Events;
using NetBlade.Core.Events.PublishStrategy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBlade.Core.EventContexts
{
    public static class EventContextExtension
    {
        public static void DisableAutoRaise(this EventContext eventContext)
        {
            eventContext.AutoRaise = false;
        }

        public static void EnableAutoRaise(this EventContext eventContext)
        {
            eventContext.AutoRaise = true;
        }

        public static bool EventIsAutoRaise(this EventContext eventContext, Event @event)
        {
            return eventContext.AutoRaise || @event is IAutoRaiseEvent;
        }

        public static Task RegisterEvent(this EventContext eventContext, Event @event)
        {
            return eventContext.RegisterEvents(new List<Event> { @event });
        }

        public static Task RegisterEvents(this EventContext eventContext, Entity entity)
        {
            return eventContext.RegisterEvents(entity.GetEvents());
        }

        public static Task RegisterEvents(this EventContext eventContext, IEnumerable<Event> events)
        {
            return eventContext.RegisterEvents(events.ToArray());
        }

        public static Task RegisterEvents(this EventContext eventContext, IEnumerable<Entity> entities)
        {
            return eventContext.RegisterEvents(entities.ToArray());
        }

        public static async Task RegisterEvents(this EventContext eventContext, params Entity[] entities)
        {
            if (entities != null && entities.Any())
            {
                List<Event> events = new List<Event>();
                foreach (Entity entity in entities)
                {
                    if (entity != null && entity.HasEvents())
                    {
                        events.AddRange(entity.GetEvents());
                    }
                }

                await eventContext.RegisterEvents(events);
            }
        }
    }
}
