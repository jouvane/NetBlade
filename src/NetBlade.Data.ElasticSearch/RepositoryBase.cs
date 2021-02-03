using Nest;
using System;

namespace NetBlade.Data.ElasticSearch
{
    public abstract class RepositoryBase<T>
        where T : class
    {
        protected RepositoryBase(ElasticManager elasticManager)
        {
            this.Manager = elasticManager;
            this.IndexName = typeof(T).Name.ToLower();
            this.CreateIndex();
        }

        protected ElasticClient Client
        {
            get => this.Manager.GetClient();
        }

        protected string IndexName { get; }

        protected ElasticManager Manager { get; }

        protected void CreateIndex(string name, int replicas = 1, int shards = 1)
        {
            ExistsResponse response = this.Manager.GetClient().Indices.Exists(new IndexExistsRequest(name));
            if (response.Exists)
            {
                return;
            }

            IndexState settings = new IndexState
            {
                Settings = new IndexSettings
                {
                    NumberOfReplicas = replicas,
                    NumberOfShards = shards
                }
            };

            this.Manager
               .GetClient()
               .Indices
               .Create(name,
                       c => c.Index(name)
                          .InitializeUsing(settings)
                          .Map(m => m.AutoMap()));
        }

        private void CreateIndex()
        {
            IndexConfiguration indexConfiguration = this.Manager.GetIndexConfiguration(this.IndexName);
            if (indexConfiguration == null)
            {
                throw new Exception($"Elastic: Configuration not found for index: {this.IndexName}");
            }

            this.CreateIndex(this.IndexName, indexConfiguration.Replicas, indexConfiguration.Shards);
        }
    }
}
