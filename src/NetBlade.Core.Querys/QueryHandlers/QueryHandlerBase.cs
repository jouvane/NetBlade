using FluentValidation.Results;

namespace NetBlade.Core.Querys
{
    public abstract class QueryHandlerBase
    {
        protected internal virtual ValidationResult ValidationResult { get; set; }
    }
}
