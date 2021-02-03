using System;

namespace NetBlade.Core.Commands
{
    public class CommandResponse<TData> : ICommandResponse, IResponse<TData>
    {
        public CommandResponse(TData data)
            : this(true)
        {
            this.Data = data;
        }

        protected CommandResponse(bool success)
        {
            this.Success = success;
        }

        public TData Data { get; set; }

        public virtual Exception Exception { get; private set; }

        public virtual bool Success { get; private set; }

        public virtual TException GetException<TException>()
            where TException : Exception
        {
            return (TException)this.Exception;
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