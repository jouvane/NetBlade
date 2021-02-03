using System;

namespace NetBlade.Core.Commands
{
    public interface ICommandResponse : IResponse
    {
        TCommandResponse SetException<TCommandResponse>(Exception ex)
            where TCommandResponse : ICommandResponse;
    }
}
