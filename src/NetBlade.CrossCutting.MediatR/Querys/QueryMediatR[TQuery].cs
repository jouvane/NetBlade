using MediatR;
using NetBlade.Core.Querys;

namespace NetBlade.CrossCutting.MediatR.Querys
{
    public class QueryMediatR : IRequest<IQueryResponse>
    {
    }

    internal class QueryMediatR<TQuery> : QueryMediatR
        where TQuery : class, IQuery
    {
        internal QueryMediatR(TQuery baseQuery)
        {
            this.BaseQuery = baseQuery;
        }

        internal TQuery BaseQuery { get; }
    }
}