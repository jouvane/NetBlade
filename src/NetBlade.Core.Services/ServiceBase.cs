using FluentValidation.Results;
using NetBlade.Core.Domain;
using NetBlade.Core.EventContexts;
using NetBlade.Core.Events;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBlade.Core.Services
{
    public abstract class ServiceBase
    {
        private readonly EventContext _eventContext;

        protected ServiceBase(EventContext eventContext, IMediatorHandler mediatorHandler)
        {
            this._eventContext = eventContext;
            this.Bus = mediatorHandler;
        }

        public virtual EventContext EventContext
        {
            get => this._eventContext;
        }

        protected internal virtual IMediatorHandler Bus { get; }

        protected internal virtual List<Event> GetEvents(IEnumerable<Entity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return null;
            }

            List<Event> events = new List<Event>();
            foreach (Entity entity in entities)
            {
                if (entity != null && entity.HasEvents())
                {
                    events.AddRange(entity.GetEvents());
                }
            }

            return events;
        }

        protected internal virtual DomainException ParserValidatorResult<T>(ValidationResult result)
        {
            if (!result.IsValid && result.Errors.Any())
            {
                ValidationResultDomainException domainException = new ValidationResultDomainException();
                domainException.AddValidationsErrors(result.Errors);

                return domainException;
            }

            return null;
        }

        protected internal virtual async Task RaiseEvents(IEnumerable<Entity> entities, IEnumerable<Event> events = null)
        {
            if ((entities == null || !entities.Any()) && (events == null || !events.Any()))
            {
            }
            else
            {
                List<Event> evts = this.GetEvents(entities) ?? new List<Event>();
                if (events != null && events.Any())
                {
                    evts.AddRange(events);
                }

                if (evts == null || !evts.Any())
                {
                }
                else
                {
                    await this._eventContext.RegisterEvents(evts);
                }
            }
        }

        protected internal virtual async Task RaiseEvents(IList<Event> events)
        {
            await this._eventContext.RegisterEvents(events);
        }
    }
}
