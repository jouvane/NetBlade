namespace NetBlade.Core.Exceptions.Test.Model
{
    public class AlertaDomainException : DomainException, INotRollbackTransaction
    {
        public AlertaDomainException(string msg)
            : base(msg)
        {
        }
    }
}
