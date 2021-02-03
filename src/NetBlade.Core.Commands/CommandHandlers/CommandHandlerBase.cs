using FluentValidation.Results;

namespace NetBlade.Core.Commands
{
    public abstract class CommandHandlerBase
    {
        protected internal virtual ValidationResult ValidationResult { get; set; }
    }
}
