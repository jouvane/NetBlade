namespace NetBlade.Core.Exceptions
{
    public class NotFoundDomainException : DomainException
    {
        public NotFoundDomainException()
        {
        }

        public NotFoundDomainException(string message)
            : base(message)
        {
        }
    }
}
