using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Transaction;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands.Test.Model.CommandHandlers
{
    public class SalvarPessoa3CommandHandler : CommandHandlerBase<SalvarPessoa3Command, ICommandResponse>
    {
        public SalvarPessoa3CommandHandler(ITransactionManager transactionManager)
            : base(transactionManager)
        {
        }

        public bool IsValid(string nome)
        {
            return !string.IsNullOrEmpty(nome);
        }

        protected override async Task<ICommandResponse> HandleAsync(SalvarPessoa3Command command)
        {
            if (this.IsValid(command.Nome))
            {
                ICommandResponse result = this.CommandResponseOk();
                return await Task.Factory.StartNew(() => result);
            }

            ICommandResponse resultFail = this.CommandResponseFail();
            return resultFail;
        }
    }
}
