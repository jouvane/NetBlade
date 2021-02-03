using NetBlade.Core.Commands;
using NetBlade.Core.Domain.Test.Events.Model.Commands;
using NetBlade.Core.Domain.Test.Events.Model.Services;
using NetBlade.Core.Transaction;
using System.Threading.Tasks;

namespace NetBlade.Core.Domain.Test.Events.Model.CommandHandlers
{
    public class AtualizarValorUnitarioCommandHandler : CommandHandlerBase<AtualizarValorUnitarioCommand, CommandResponse>
    {
        private readonly ProdutoServices _produtoServices;

        public AtualizarValorUnitarioCommandHandler(ITransactionManager transactionManager, ProdutoServices produtoServices)
            : base(transactionManager)
        {
            this._produtoServices = produtoServices;
        }

        protected override async Task<CommandResponse> HandleAsync(AtualizarValorUnitarioCommand command)
        {
            await this._produtoServices.AtualizarValorUnitario(command.Codigo, command.Valor);
            CommandResponse result = this.CommandResponseOk<CommandResponse>();

            return result;
        }
    }
}
