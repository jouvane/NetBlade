namespace NetBlade.CrossCutting.ClientFactory.WCF.ConfigurationOption
{
    public class BindingConfigurationsOption
    {
        public BindingConfigurationsOption()
        {
            this.Security = new SecurityConfigurationOption();
            this.ReaderQuotas = new ReaderQuotasConfigurationOption();
        }

        public virtual int? CloseTimeout { get; set; }

        public virtual int? MaxBufferPoolSize { get; set; }

        public virtual int? MaxBufferSize { get; set; }

        public virtual int? MaxReceivedMessageSize { get; set; }

        public virtual int? OpenTimeout { get; set; }

        public virtual ReaderQuotasConfigurationOption ReaderQuotas { get; set; }

        public virtual int? ReceiveTimeout { get; set; }

        public virtual SecurityConfigurationOption Security { get; set; }

        public virtual int? SendTimeout { get; set; }

        public virtual int? TransferMode { get; set; }

        public virtual string Type { get; set; }
    }
}
