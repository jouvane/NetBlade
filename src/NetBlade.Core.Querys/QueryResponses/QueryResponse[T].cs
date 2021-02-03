using System;

namespace NetBlade.Core.Querys
{
    public class QueryResponse<TData> : IQueryResponse, IResponse<TData>
    {
        public QueryResponse(TData data)
            : this(true)
        {
            this.Data = data;
        }

        protected QueryResponse(bool success)
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

        public TQueryResponse SetException<TQueryResponse>(Exception ex)
            where TQueryResponse : IQueryResponse
        {
            this.SetException(ex);
            return (TQueryResponse)(IQueryResponse)this;
        }

        public void SetException(Exception ex)
        {
            this.Success = false;
            this.Exception = ex;
        }
    }
}