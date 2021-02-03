using NetBlade.Core.Pagination;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetBlade.Data.EF
{
    public interface IRepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        bool IsAttached(TEntity entity);

        void Load(TEntity entity);

        Task PageAsync<TResult>(IPaginationResult<TResult> pagination, Expression<Func<TEntity, TResult>> select);
    }
}