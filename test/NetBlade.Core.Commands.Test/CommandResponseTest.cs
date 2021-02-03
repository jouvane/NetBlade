using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Commands.Test.Model.CommandResponses;
using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Mediator;
using NetBlade.Core.Transaction;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.Core.Commands.Test
{
    public class CommandResponseTest : IClassFixture<Startup>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ServiceProvider _serviceProvider;

        public CommandResponseTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
            this._mediatorHandler = this._serviceProvider.GetService<IMediatorHandler>();
        }

        [Fact]
        public async Task SalvarPessoa2CommandErrorCodigoTest()
        {
            SalvarPessoa2Command cmd = this._serviceProvider.GetService<SalvarPessoa2Command>();
            cmd.Nome = "Geovane Alves Simões";
            cmd.Codigo = -5;

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);

            Assert.False(resp.Success);
            Assert.IsType<DomainException>(resp.Exception);
            Assert.Equal("Id nao permitido!", resp.Exception.Message);
        }

        [Fact]
        public async Task SalvarPessoa2CommandErrorNomeTest()
        {
            SalvarPessoa2Command cmd = this._serviceProvider.GetService<SalvarPessoa2Command>();
            cmd.Nome =
                "Geovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves Simões";
            cmd.Codigo = -5;

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);

            Assert.False(resp.Success);
            Assert.Equal("O campo \"Nome\" nao pode ser maior que 200.", resp.Exception.Message);
        }

        [Fact]
        public async Task SalvarPessoa2CommandSucessoTest()
        {
            SalvarPessoa2Command cmd = this._serviceProvider.GetService<SalvarPessoa2Command>();
            cmd.Nome = "Geovane Alves Simões";

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.Equal(1, ((SalvarPessoa2CommandResponse)resp).Codigo);
        }

        [Fact]
        public async Task SalvarPessoaCommand3ErrorTest()
        {
            SalvarPessoa3Command cmd = this._serviceProvider.GetService<SalvarPessoa3Command>();
            cmd.Nome = "";

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.False(resp.Success);
        }

        [Fact]
        public async Task SalvarPessoaCommand3SucessoTest()
        {
            SalvarPessoa3Command cmd = this._serviceProvider.GetService<SalvarPessoa3Command>();
            cmd.Nome = "Geovane Alves Simões";

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.True(resp.Success);
        }

        [Fact]
        public async Task SalvarPessoaCommand4ErrorTest()
        {
            SalvarPessoa4Command cmd = this._serviceProvider.GetService<SalvarPessoa4Command>();
            cmd.Nome = "";

            CommandResponse<int> resp = await this._mediatorHandler.SendCommand<int>(cmd);
            Assert.False(resp.Success);
        }

        [Fact]
        public async Task SalvarPessoaCommand4SucessoTest()
        {
            SalvarPessoa4Command cmd = this._serviceProvider.GetService<SalvarPessoa4Command>();
            cmd.Nome = "Geovane Alves Simões";

            CommandResponse<int> resp = await this._mediatorHandler.SendCommand<int>(cmd);
            Assert.True(resp.Success);
            Assert.Equal(10, resp.Data);
        }

        [Fact]
        public async Task SalvarPessoaCommandErrorCodigoTest()
        {
            SalvarPessoaCommand cmd = this._serviceProvider.GetService<SalvarPessoaCommand>();
            cmd.Nome = "Geovane Alves Simões";
            cmd.Codigo = -5;

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);

            Assert.False(resp.Success);
            Assert.IsType<DomainException>(resp.Exception);
            Assert.Equal("Id nao permitido!", resp.Exception.Message);
        }

        [Fact]
        public async Task SalvarPessoaCommandErrorNomeTest()
        {
            SalvarPessoaCommand cmd = this._serviceProvider.GetService<SalvarPessoaCommand>();
            cmd.Nome =
                "Geovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves SimõesGeovane Alves Simões Geovane Alves Simões";
            cmd.Codigo = -5;

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);

            Assert.False(resp.Success);
            Assert.Equal("O campo \"Nome\" nao pode ser maior que 200.", resp.Exception.Message);
        }

        [Fact]
        public async Task SalvarPessoaCommandSucessoTest()
        {
            SalvarPessoaCommand cmd = this._serviceProvider.GetService<SalvarPessoaCommand>();
            cmd.Nome = "Geovane Alves Simões";

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.Equal(1, ((SalvarPessoaCommandResponse)resp).Codigo);
        }

        [Fact]
        public async Task SalvarPessoaRequiresNewTransactionScopeCommandTest()
        {
            SalvarPessoaRequiresNewTransactionScopeCommand cmd = this._serviceProvider.GetService<SalvarPessoaRequiresNewTransactionScopeCommand>();
            ITransactionManager transactionManager = this._serviceProvider.GetService<ITransactionManager>();
            cmd.Valid = (ITransactionManager transactionManagerNew) =>
            {
                Assert.NotEqual(transactionManager, transactionManagerNew);
            };

            ICommandResponse resp = await this._mediatorHandler.SendCommand(cmd);
            Assert.True(resp.Success);
        }
    }
}
