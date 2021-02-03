using System.Collections.Generic;

namespace NetBlade.CrossCutting.ClientFactory.WCF.ConfigurationOption
{
    public class ServicesConfigurationOption
    {
        public virtual Dictionary<string, BindingConfigurationsOption> Bindings { get; set; }

        public virtual Dictionary<string, EndPointConfigurationOption> EndPoints { get; set; }
    }
}
