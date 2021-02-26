using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetBlade.Core.Transaction;
using System;
using System.Data;

namespace NetBlade.Data.EF
{
    public class TransactionManagerEF : ITransactionManager
    {
        private static int count = 0;
        private readonly IConnectionManager _connection;
        private readonly DbContext _context;
        private IDbContextTransaction _contextTransaction;
        private bool _readOnly;
        private bool _started;
        private int id = 0;

        public TransactionManagerEF(DbContext context, IConnectionManager connection)
        {
            this._connection = connection;
            this._context = context;
            TransactionManagerEF.count++;
            this.id = TransactionManagerEF.count;
        }

        public IDbConnection Connection =>
            this._context.Database.CurrentTransaction?.GetDbTransaction()?.Connection ?? this._connection.Open();

        public virtual bool Started =>
            this._started;

        public IDbTransaction Transaction =>
            this._context.Database.CurrentTransaction.GetDbTransaction();

        public virtual void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (!this._started)
            {
                this._started = true;
                this._contextTransaction = this._context.Database.BeginTransaction(isolationLevel);
            }
        }

        public virtual void Commit()
        {
            if (!this._readOnly)
            {
                this._contextTransaction?.Commit();
                this._started = false;
            }
        }

        public virtual void Dispose()
        {
            this._contextTransaction?.Dispose();
            this._started = false;
        }

        public virtual void ReadOnly(bool readOnly)
        {
            this._readOnly = readOnly;
        }

        public virtual void Rollback()
        {
            if (this._started)
            {
                this._contextTransaction?.Rollback();
                this._started = false;
            }
        }

        public void SetTransaction(IDbTransaction transaction) =>
            throw new NotImplementedException();
    }
}
