using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;

namespace NetBlade.Data.ElasticSearch
{
    public abstract class ElasticConnection : IElasticConnection
    {
        private readonly IOptions<List<Uri>> _uris;
        private ElasticClient _client;

        public ElasticConnection(IOptions<List<Uri>> uris)
        {
            this._uris = uris;
        }

        public ElasticClient GetClient()
        {
            if (this._client != null)
            {
                return this._client;
            }

            this._client = new ElasticClient(this.GetSettings());
            return this._client;
        }

        protected virtual ConnectionSettings GetSettings(StaticConnectionPool pool)
        {
            return new ConnectionSettings(pool).DefaultFieldNameInferrer(s => s);
        }

        private ConnectionSettings GetSettings()
        {
            StaticConnectionPool pool = new StaticConnectionPool(this._uris.Value);
            return this.GetSettings(pool);
        }
    }
}
