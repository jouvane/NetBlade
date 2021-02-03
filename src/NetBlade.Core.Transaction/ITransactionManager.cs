using System;
using System.Data;

namespace NetBlade.Core.Transaction
{
    public interface ITransactionManager : IDisposable
    {
        IDbConnection Connection { get; }

        bool Started { get; }

        IDbTransaction Transaction { get; }

        void Begin(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Commit();

        void ReadOnly(bool readOnly);

        void Rollback();

        void SetTransaction(IDbTransaction transaction);
    }
}
