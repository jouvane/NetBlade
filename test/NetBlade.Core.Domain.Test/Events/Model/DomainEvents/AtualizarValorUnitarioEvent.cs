using NetBlade.Core.Events;

namespace NetBlade.Core.Domain.Test.Events.Model.DomainEvents
{
    public class AtualizarValorUnitarioEvent : DomainEvent
    {
        public AtualizarValorUnitarioEvent(Entity entity, double oldValue)
            : base(entity, Enum.OperationType.Update)
        {
            this.OldValue = oldValue;
        }

        public double OldValue { get; set; }
    }
}
