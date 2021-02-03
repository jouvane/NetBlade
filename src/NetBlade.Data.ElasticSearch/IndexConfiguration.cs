namespace NetBlade.Data.ElasticSearch
{
    public class IndexConfiguration
    {
        public int MergeFactor { get; set; }

        public string Name { get; set; }

        public int Replicas { get; set; }

        public int Shards { get; set; }

        public string SlowLogThresholdFetchWarn { get; set; }
    }
}
