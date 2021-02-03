using NetBlade.Core.Commands.Test.Model.CommandResponses;
using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Commands.Test.Model.Validations;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Transaction;
using System;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands.Test.Model.CommandHandlers
{
    public class SalvarPessoaCommandHandler : CommandHandlerBase<SalvarPessoaCommand, SalvarPessoaCommandResponse>
    {
        public SalvarPessoaCommandHandler(ITransactionManager transactionManager, SalvarPessoaValidation validation)
            : base(transactionManager, validation)
        {
        }

        protected override async Task<SalvarPessoaCommandResponse> HandleAsync(SalvarPessoaCommand command)
        {
            if (command.Codigo <= -1)
            {
                throw new DomainException("Id nao permitido!");
            }

            SalvarPessoaCommandResponse result = this.CommandResponseOk<SalvarPessoaCommandResponse>();
            result.Contrato = Guid.NewGuid().ToString();
            result.Codigo = command.Codigo ?? 1;

            return await Task.Factory.StartNew(() => result);
        }
    }
}
