using System.Threading.Tasks;

namespace NetBlade.Data
{
    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        int Count();

        Task<int> CountAsync();

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        TEntity Get(object id);

        Task<TEntity> GetAsync(object id);

        void Save(TEntity entity);

        Task SaveAsync(TEntity entity);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);
    }
}