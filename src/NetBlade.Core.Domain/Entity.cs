using NetBlade.Core.Events;
using System.Collections.Generic;
using System.Linq;

namespace NetBlade.Core.Domain
{
    public abstract class Entity
    {
        internal List<Event> _events = new List<Event>();

        public abstract bool IsNew { get; }

        public virtual bool HasEvents()
        {
            return this._events?.Any() ?? false;
        }
    }
}
