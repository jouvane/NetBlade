using System;

namespace NetBlade.Core.Commands
{
    public class CommandResponse : ICommandResponse
    {
        protected CommandResponse(bool success)
        {
            this.Success = success;
        }

        public virtual Exception Exception { get; private set; }

        public virtual bool Success { get; private set; }

        public virtual T GetException<T>()
            where T : Exception
        {
            return (T)this.Exception;
        }

        public TCommandResponse SetException<TCommandResponse>(Exception ex)
            where TCommandResponse : ICommandResponse
        {
            this.SetException(ex);
            return (TCommandResponse)(ICommandResponse)this;
        }

        public void SetException(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
    }
}
