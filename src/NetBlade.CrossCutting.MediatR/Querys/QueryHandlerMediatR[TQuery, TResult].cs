using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Querys;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.MediatR.Querys
{
    internal class QueryHandlerMediatR<TQuery, TQueryResponse> : IRequestHandler<QueryMediatR<TQuery>, IQueryResponse>, IDisposable
        where TQuery : class, IQuery
        where TQueryResponse : IQueryResponse
    {
        private static readonly object _lock = new object();
        private readonly QueryHandlerBase<TQuery, TQueryResponse> _queryHandler;
        private readonly IServiceScope _serviceScope;
        private bool _disposed;

        internal QueryHandlerMediatR(IServiceScopeFactory serviceScopeFactory, Type typeQueryHandler)
        {
            this._serviceScope = serviceScopeFactory.CreateScope();
            this._queryHandler = (QueryHandlerBase<TQuery, TQueryResponse>)this._serviceScope.ServiceProvider.GetService(typeQueryHandler);
        }

        internal QueryHandlerMediatR(QueryHandlerBase<TQuery, TQueryResponse> queryHandler)
        {
            this._queryHandler = queryHandler;
        }

        public void Dispose()
        {
            if (this._serviceScope != null)
            {
                bool disposed;
                lock (QueryHandlerMediatR<TQuery, TQueryResponse>._lock)
                {
                    disposed = this._disposed;
                    this._disposed = true;
                }

                if (!disposed)
                {
                    this._serviceScope.Dispose();
                }
            }
        }

        public async Task<IQueryResponse> Handle(QueryMediatR<TQuery> request, CancellationToken cancellationToken)
        {
            return await this._queryHandler.Handle(request.BaseQuery, cancellationToken);
        }
    }
}