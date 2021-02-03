using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace NetBlade.CrossCutting.Mask.Test
{
    public class Startup
    {
        public Startup()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection);
            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ServiceProvider ServiceProvider { get; }

        private void ConfigureServices(IServiceCollection services)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true);

            this.Configuration = builder.Build();
            services.AddLogging(l => l.AddSerilog(dispose: true));

            Log.Logger = new LoggerConfiguration()
               .ReadFrom
               .Configuration(this.Configuration)
               .CreateLogger();
        }
    }
}
