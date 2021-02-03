using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace NetBlade.Data.ElasticSearch
{
    public class ElasticManager
    {
        private readonly IElasticConnection _connection;
        private readonly IOptions<List<IndexConfiguration>> _indexes;

        public ElasticManager(IElasticConnection connection, IOptions<List<IndexConfiguration>> indexes)
        {
            this._connection = connection;
            this._indexes = indexes;
        }

        public ElasticClient GetClient()
        {
            return this._connection.GetClient();
        }

        public IndexConfiguration GetIndexConfiguration(string indexName)
        {
            List<IndexConfiguration> indexes = this._indexes.Value;
            return indexes.FirstOrDefault(i => i.Name == indexName);
        }
    }
}
