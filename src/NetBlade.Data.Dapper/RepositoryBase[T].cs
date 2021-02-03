using Dapper;
using NetBlade.Core.Transaction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBlade.Data.Dapper
{
    public class RepositoryBase<T> : IRepository
    {
        protected RepositoryBase(ITransactionManager transactionManager)
        {
            this.TransactionManager = transactionManager;
        }

        public virtual ITransactionManager TransactionManager { get; }

        public virtual IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id")
        {
            return this.TransactionManager
               .Connection
               .Query(sql, map, param, this.TransactionManager.Transaction, splitOn: splitOn);
        }

        public virtual IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return this.TransactionManager
               .Connection
               .Query(sql, map, param, this.TransactionManager.Transaction, splitOn: splitOn);
        }

        public virtual SqlMapper.GridReader QueryMultiple(string sql, object param = null)
        {
            return this.TransactionManager
               .Connection
               .QueryMultiple(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual int Execute(string sql, object param = null)
        {
            return this.TransactionManager
               .Connection
               .Execute(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual async Task<int> ExecuteAsync(string sql, object param = null)
        {
            return await this.TransactionManager
               .Connection
               .ExecuteAsync(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null)
        {
            return await this.TransactionManager
               .Connection
               .ExecuteScalarAsync<TReturn>(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual IEnumerable<TReturn> Query<TReturn>(string sql, object param = null)
        {
            return this.TransactionManager
               .Connection
               .Query<TReturn>(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual IEnumerable<dynamic> Query(string sql, object param = null)
        {
            return this.TransactionManager
               .Connection
               .Query(sql, param, this.TransactionManager.Transaction);
        }

        protected virtual IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
        {
            return this.TransactionManager
               .Connection
               .Query(sql, map, param, this.TransactionManager.Transaction, splitOn: splitOn);
        }

        protected virtual async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, object param = null)
        {
            return await this.TransactionManager
               .Connection
               .QueryAsync<TReturn>(sql, param, this.TransactionManager.Transaction);
        }
    }
}