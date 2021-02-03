namespace NetBlade.CrossCutting.ClientFactory.WCF.ConfigurationOption
{
    public class ReaderQuotasConfigurationOption
    {
        public virtual int? MaxArrayLength { get; set; }

        public virtual int? MaxBytesPerRead { get; set; }

        public virtual int? MaxDepth { get; set; }

        public virtual int? MaxNameTableCharCount { get; set; }

        public virtual int? MaxStringContentLength { get; set; }
    }
}
