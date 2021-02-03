using Nest;

namespace NetBlade.Data.ElasticSearch
{
    public interface IElasticConnection
    {
        ElasticClient GetClient();
    }
}
