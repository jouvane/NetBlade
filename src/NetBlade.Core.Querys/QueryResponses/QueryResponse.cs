using System;

namespace NetBlade.Core.Querys
{
    public class QueryResponse : IQueryResponse
    {
        protected QueryResponse(bool success)
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
