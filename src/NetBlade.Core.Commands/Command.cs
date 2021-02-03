using System;

namespace NetBlade.Core.Commands
{
    public abstract class Command : ICommand
    {
        protected Command()
        {
            this.TimestampCommand = DateTime.Now;
            this.IdCommand = Guid.NewGuid().ToString();
        }

        protected string IdCommand { get; }

        protected DateTime TimestampCommand { get; }
    }
}
