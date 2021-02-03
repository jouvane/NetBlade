using System;

namespace NetBlade.Core.Events
{
    public abstract class Event : Message
    {
        protected Event()
        {
            this.IdEvent = Guid.NewGuid();
            this.TimestampEvent = DateTime.Now;
        }

        public Guid IdEvent { get; }

        public bool Raised { get; set; }

        public DateTime TimestampEvent { get; }
    }
}
