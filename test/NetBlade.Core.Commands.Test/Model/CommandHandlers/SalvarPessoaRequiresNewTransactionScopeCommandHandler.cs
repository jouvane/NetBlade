using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Transaction;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands.Test.Model.CommandHandlers
{
    public class SalvarPessoaRequiresNewTransactionScopeCommandHandler : CommandHandlerBase<SalvarPessoaRequiresNewTransactionScopeCommand, CommandResponse>
    {
        public SalvarPessoaRequiresNewTransactionScopeCommandHandler(ITransactionManager transactionManager)
            : base(transactionManager)
        {
        }

        protected override Task<CommandResponse> HandleAsync(SalvarPessoaRequiresNewTransactionScopeCommand command)
        {
            command?.Valid(this.TransactionManager);
            return Task.FromResult(this.CommandResponseOk());
        }
    }
}
