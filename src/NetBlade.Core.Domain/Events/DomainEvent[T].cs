using NetBlade.Core.Domain;
using NetBlade.Core.Enum;

namespace NetBlade.Core.Events
{
    public class DomainEvent<T> : DomainEvent
        where T : Entity
    {
        internal DomainEvent(T entity, OperationType? operationType = null)
            : base(entity, operationType)
        {
        }

        public static DomainEvent<T> Create(T entity, OperationType operationType)
        {
            return new DomainEvent<T>(entity, operationType);
        }

        public static DomainEvent<T> Delete(T entity)
        {
            return DomainEvent<T>.Create(entity, Enum.OperationType.Delete);
        }

        public static DomainEvent<T> Insert(T entity)
        {
            return DomainEvent<T>.Create(entity, Enum.OperationType.Insert);
        }

        public static DomainEvent<T> Update(T entity)
        {
            return DomainEvent<T>.Create(entity, Enum.OperationType.Update);
        }
    }
}