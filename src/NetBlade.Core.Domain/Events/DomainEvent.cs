using NetBlade.Core.Domain;
using NetBlade.Core.Enum;

namespace NetBlade.Core.Events
{
    public abstract class DomainEvent : Event
    {
        protected DomainEvent(Entity entity, OperationType? operationType = null)
        {
            this.Entity = entity;
            this.OperationType = operationType;
        }

        public Entity Entity { get; }

        public OperationType? OperationType { get; set; }

        public T EntityAs<T>()
            where T : class
        {
            return this.Entity != null ? this.Entity as T : default;
        }
    }
}
