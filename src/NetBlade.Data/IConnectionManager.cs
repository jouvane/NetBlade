using System;
using System.Data;

namespace NetBlade.Data
{
    public interface IConnectionManager : IDisposable
    {
        void Close();

        IDbConnection Get();

        IDbConnection Open();

        void SetConnection(IDbConnection connection);
    }
}
