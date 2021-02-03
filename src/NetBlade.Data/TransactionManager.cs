using NetBlade.Core.Transaction;
using System.Data;

namespace NetBlade.Data
{
    public class TransactionManager : ITransactionManager
    {
        public static int count;
        public int id;
        private bool _readOnly;
        private bool _started;

        public TransactionManager(IConnectionManager connectionManager)
        {
            TransactionManager.count++;
            this.id = TransactionManager.count;
            this.ConnectionManager = connectionManager;
        }

        public virtual IConnectionManager ConnectionManager { get; }

        public virtual void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (!this._started)
            {
                this._started = true;
                this.Transaction = this.ConnectionManager.Open().BeginTransaction(isolationLevel);
            }
        }

        public virtual void Commit()
        {
            if (!this._readOnly)
            {
                this.Transaction?.Commit();
                this._started = false;
            }
        }

        public virtual IDbConnection Connection
        {
            get => this.ConnectionManager.Get();
        }

        public virtual void Dispose()
        {
            this.Transaction?.Dispose();
        }

        public virtual void ReadOnly(bool readOnly)
        {
            this._readOnly = readOnly;
        }

        public virtual void Rollback()
        {
            this.Transaction?.Rollback();
            this._started = false;
        }

        public virtual void SetTransaction(IDbTransaction transaction)
        {
            this.Transaction = transaction;
        }

        public virtual bool Started
        {
            get => this._started;
        }

        public virtual IDbTransaction Transaction { get; private set; }
    }
}
