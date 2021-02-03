using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Movies.API
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureAppConfiguration(delegate (WebHostBuilderContext hostingContext, IConfigurationBuilder config)
                    {
                        config
                            .AddJsonFile("env.json", optional: true, reloadOnChange: true);
                    });
                });
        }

        public static void Main(string[] args)
        {
            Program.CreateHostBuilder(args).Build().Run();
        }
    }
}
