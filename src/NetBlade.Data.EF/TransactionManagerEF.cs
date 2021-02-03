using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetBlade.Core.Transaction;
using System;
using System.Data;

namespace NetBlade.Data.EF
{
    public class TransactionManagerEF : ITransactionManager
    {
        public static int count;
        private readonly DbContext _context;
        public int id;
        private IDbContextTransaction _contextTransaction;
        private bool _readOnly;
        private bool _started;

        public TransactionManagerEF(DbContext context)
        {
            TransactionManagerEF.count++;
            this.id = TransactionManagerEF.count;
            this._context = context;
        }

        public virtual void Begin(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
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

        public IDbConnection Connection
        {
            get => this.Connection;
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

        public void SetTransaction(IDbTransaction transaction)
        {
            //RelationalTransactionFactory a;
            //a.Create(null, transaction, Guid, logger, Owned)

            //this._context.Database.UseTransaction()
            throw new NotImplementedException();
        }

        public virtual bool Started
        {
            get => this._started;
        }

        public IDbTransaction Transaction
        {
            get => throw new NotImplementedException();
        }
    }
}
