using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Transaction;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands.Test.Model.CommandHandlers
{
    public class SalvarPessoa4CommandHandler : CommandHandlerBase<SalvarPessoa4Command, CommandResponse<int>>
    {
        public SalvarPessoa4CommandHandler(ITransactionManager transactionManager)
            : base(transactionManager)
        {
        }

        public bool IsValid(string nome)
        {
            return !string.IsNullOrEmpty(nome);
        }

        protected override async Task<CommandResponse<int>> HandleAsync(SalvarPessoa4Command command)
        {
            if (this.IsValid(command.Nome))
            {
                CommandResponse<int> result = this.CommandResponseOk(10);
                return await Task.Factory.StartNew(() => result);
            }

            CommandResponse<int> resultFail = this.CommandResponseFail<CommandResponse<int>>();
            return resultFail;
        }
    }
}
