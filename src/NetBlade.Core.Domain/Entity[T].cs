namespace NetBlade.Core.Domain
{
    public abstract class Entity<TID> : Entity
    {
        public virtual TID ID { get; set; }

        public override bool IsNew
        {
            get => default(TID).Equals(this.ID);
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode() * 907 + this.ID.GetHashCode();
        }
    }
}