using System;

namespace NetBlade.Core.Querys
{
    public interface IQueryResponse : IResponse
    {
        TQueryResponse SetException<TQueryResponse>(Exception ex)
            where TQueryResponse : IQueryResponse;
    }
}
