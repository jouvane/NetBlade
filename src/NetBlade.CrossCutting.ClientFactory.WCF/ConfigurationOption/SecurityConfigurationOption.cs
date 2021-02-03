namespace NetBlade.CrossCutting.ClientFactory.WCF.ConfigurationOption
{
    public class SecurityConfigurationOption
    {
        public virtual int? MessageClientCredentialType { get; set; }

        public virtual int? Mode { get; set; }

        public virtual int? TransportClientCredentialType { get; set; }
    }
}
