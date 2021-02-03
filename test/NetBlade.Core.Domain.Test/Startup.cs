using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetBlade.Core.Commands;
using NetBlade.Core.Domain.Test.Events.Model;
using NetBlade.Core.Domain.Test.Events.Model.CommandHandlers;
using NetBlade.Core.Domain.Test.Events.Model.Commands;
using NetBlade.Core.Domain.Test.Events.Model.DomainEvents;
using NetBlade.Core.Domain.Test.Events.Model.EventHandlers;
using NetBlade.Core.Domain.Test.Events.Model.Services;
using NetBlade.Core.Transaction;
using NetBlade.CrossCutting.MediatR;
using Serilog;

namespace NetBlade.Core.Domain.Test
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

            services.AddScoped<ProdutoServices>();
            services.AddScoped(s => Mock.Of<ITransactionManager>());

            services.AddMediatoInMemory(config => { config.AsScoped(); })
               .AddCommand<AtualizarValorUnitarioCommand, AtualizarValorUnitarioCommandHandler, CommandResponse>()
               .AddEvent<AtualizarValorUnitarioEvent, MensagemChatEventHandler>()
               .AddEvent<AtualizarValorUnitarioEvent, PromocaoEmailEventHandler>();

            services.AddScoped(s => Mock.Of<ISendMsgTest>());
        }
    }
}
