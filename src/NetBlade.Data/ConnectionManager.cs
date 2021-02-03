using System.Data;

namespace NetBlade.Data
{
    public class ConnectionManager : IConnectionManager
    {
        public static int count;
        public int id;
        private IDbConnection _connection;

        public ConnectionManager(IDbConnection connection)
        {
            ConnectionManager.count++;
            this.id = ConnectionManager.count;
            this._connection = connection;
        }

        public virtual string ConnectionString
        {
            get => this._connection?.ConnectionString;
        }

        public virtual void Close()
        {
            this._connection?.Close();
        }

        public virtual void Dispose()
        {
            this._connection?.Dispose();
        }

        public IDbConnection Get()
        {
            return this._connection;
        }

        public virtual IDbConnection Open()
        {
            IDbConnection connection = this.Get();
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }

            return connection;
        }

        public virtual void SetConnection(IDbConnection connection)
        {
            this._connection = connection;
        }
    }
}
