using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NetBlade.Core.Commands.Test.Model.CommandHandlers;
using NetBlade.Core.Commands.Test.Model.CommandResponses;
using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Commands.Test.Model.Validations;
using NetBlade.Core.Transaction;
using NetBlade.CrossCutting.MediatR;
using Serilog;
using System;

namespace NetBlade.Core.Commands.Test
{
    public class Startup : IDisposable
    {
        public Startup()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection);
            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            this.ServiceProvider.Dispose();
        }

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

            services.AddScoped(s => Mock.Of<ITransactionManager>());
            services.AddMediatoInMemory(config => { config.AsScoped(); })
               .AddCommand<SalvarPessoaCommand, SalvarPessoaCommandHandler, SalvarPessoaCommandResponse>()
               .AddCommand<SalvarPessoa2Command, SalvarPessoa2CommandHandler, SalvarPessoa2CommandResponse>()
               .AddCommand<SalvarPessoa3Command, SalvarPessoa3CommandHandler, ICommandResponse>()
               .AddCommand<SalvarPessoa4Command, SalvarPessoa4CommandHandler, CommandResponse<int>>()
               .AddCommand<SalvarPessoaRequiresNewTransactionScopeCommand, SalvarPessoaRequiresNewTransactionScopeCommandHandler, CommandResponse>();

            services
               .AddScoped<SalvarPessoaValidation>()
               .AddScoped<SalvarPessoa2Validation>();
        }
    }
}
