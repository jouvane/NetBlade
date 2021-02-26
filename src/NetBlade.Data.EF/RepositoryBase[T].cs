using Microsoft.EntityFrameworkCore;
using NetBlade.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetBlade.Data.EF
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        public RepositoryBase(DbContext context)
        {
            this.Context = context;
        }

        protected DbContext Context { get; }

        protected DbSet<TEntity> EntitySet
        {
            get => this.Context.Set<TEntity>();
        }

        public virtual int Count()
        {
            return this.EntitySet.Count();
        }

        public virtual async Task<int> CountAsync()
        {
            return await this.EntitySet.CountAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            this.EntitySet.Remove(entity);
            this.Context.SaveChanges();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            this.EntitySet.Remove(entity);
            await this.Context.SaveChangesAsync();
        }

        public virtual TEntity Get(object id)
        {
            return this.EntitySet.Find(id);
        }

        public virtual async Task<TEntity> GetAsync(object id)
        {
            return await this.EntitySet.FindAsync(id);
        }

        public virtual bool IsAttached(TEntity entity)
        {
            return this.Context.Entry(entity).State != EntityState.Detached;
        }

        public virtual void Load(TEntity entity)
        {
            this.Context.Entry(entity).Reload();
        }

        public virtual async Task PageAsync<TResult>(IPaginationResult<TResult> pagination, Expression<Func<TEntity, TResult>> select)
        {
            await this.PageAsync(this.EntitySet, pagination, select, null);
        }

        public virtual void Save(TEntity entity)
        {
            this.Update(entity);
        }

        public virtual async Task SaveAsync(TEntity entity)
        {
            await this.UpdateAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (!this.IsAttached(entity))
            {
                this.EntitySet.Add(entity);
            }

            this.Context.SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (!this.IsAttached(entity))
            {
                this.EntitySet.Add(entity);
            }

            await this.Context.SaveChangesAsync();
        }

        public virtual List<TEntity> ListAll()
        {
            return this.EntitySet.ToList();
        }

        public virtual async Task<List<TEntity>> ListAllAsync()
        {
            return await this.EntitySet.ToListAsync();
        }

        public virtual async Task LoadAsync(TEntity entity)
        {
            await this.Context.Entry(entity).ReloadAsync();
        }

        protected virtual IOrderedQueryable<TEntity> OrderByPropertyName(IQueryable<TEntity> query, string propertyName, bool orderAscending)
        {
            return this.OrderByPropertyName<TEntity>(query, propertyName, orderAscending);
        }

        protected virtual IOrderedQueryable<T> OrderByPropertyName<T>(IQueryable<T> query, string propertyName, bool orderAscending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            Type type = typeof(T);
            ParameterExpression parameterExpression = Expression.Parameter(type, "x");
            Expression expression = parameterExpression;
            string[] array = propertyName.Split('.');

            foreach (string propertyOrFieldName in array)
            {
                expression = Expression.PropertyOrField(expression, propertyOrFieldName);
                type = expression.Type;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambdaExpression = Expression.Lambda(delegateType, expression, parameterExpression);
            string methodName = orderAscending ? "OrderBy" : "OrderByDescending";

            object obj = typeof(Queryable)
               .GetMethods()
               .First(method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2)
               .MakeGenericMethod(typeof(T), type)
               .Invoke(null, new object[2]
                {
                    query.AsQueryable(),
                    lambdaExpression
                });

            return (IOrderedQueryable<T>)obj;
        }

        protected virtual async Task PageAsync<TSource>(IQueryable<TSource> query, IPaginationResult<TSource> pagination)
        {
            await this.PageAsync(query, pagination, s => s, null);
        }

        protected virtual async Task PageAsync<TSource, TResult, TProjec>(IQueryable<TSource> query, IPaginationResult<TResult> pagination, Expression<Func<TSource, TProjec>> selectA, Expression<Func<TProjec, TResult>> selectB)
        {
            List<TProjec> array = await (await this.PageBuidQueryAsync(query, pagination)).Select(selectA).ToListAsync();
            if (selectB != null)
            {
                pagination.Entities = array.Select(s => selectB.Compile()(s)).ToList();
            }
            else
            {
                pagination.Entities = (List<TResult>)(object)array;
            }
        }

        protected virtual async Task<IQueryable<TQuery>> PageBuidQueryAsync<TQuery, TResult>(IQueryable<TQuery> query, IPaginationResult<TResult> pagination)
        {
            if (pagination.PageSize == 0)
            {
                pagination.PageSize = 10;
            }

            int count = await this.PageCountAsync(query, pagination);
            return this.OrderByPropertyName(query, pagination.SortField, "ASC".Equals((pagination.SortDirection ?? "ASC").ToUpper()))
               .Skip(count)
               .Take(pagination.PageSize);
        }

        protected virtual async Task<int> PageCountAsync<TQuery, TResult>(IQueryable<TQuery> query, IPaginationResult<TResult> pagination)
        {
            if (pagination.ActualPage == 0)
            {
                pagination.ActualPage = 1;
            }

            pagination.RowsCount = await query.CountAsync();
            int num = (pagination.ActualPage - 1) * pagination.PageSize;

            if (num > pagination.RowsCount)
            {
                num = 0;
                pagination.ActualPage = 1;
            }

            return num;
        }
    }
}