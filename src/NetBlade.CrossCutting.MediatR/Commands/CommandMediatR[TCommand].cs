using MediatR;
using NetBlade.Core.Commands;

namespace NetBlade.CrossCutting.MediatR.Commands
{
    public class CommandMediatR : IRequest<ICommandResponse>
    {
    }

    public class CommandMediatR<TCommand> : CommandMediatR
        where TCommand : class, ICommand
    {
        internal CommandMediatR(TCommand baseCommand)
        {
            this.BaseCommand = baseCommand;
        }

        internal TCommand BaseCommand { get; }
    }
}