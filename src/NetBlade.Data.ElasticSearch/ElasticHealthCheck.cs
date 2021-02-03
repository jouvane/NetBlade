using Elasticsearch.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Data.ElasticSearch
{
    public class ElasticHealthCheck : IHealthCheck
    {
        private readonly ElasticManager _manager;

        public ElasticHealthCheck(ElasticManager manager)
        {
            this._manager = manager;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            ClusterHealthResponse response = await this._manager.GetClient().Cluster.HealthAsync(Indices.AllIndices);

            if (!response.ApiCall.Success)
            {
                return HealthCheckResult.Unhealthy();
            }

            switch (response.Status)
            {
                case Health.Green:
                    return HealthCheckResult.Healthy();
                case Health.Yellow:
                    return HealthCheckResult.Degraded();
                case Health.Red:
                    return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
