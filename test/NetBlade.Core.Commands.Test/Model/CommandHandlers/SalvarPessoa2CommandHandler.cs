using NetBlade.Core.Commands.Test.Model.CommandResponses;
using NetBlade.Core.Commands.Test.Model.Commands;
using NetBlade.Core.Commands.Test.Model.Validations;
using NetBlade.Core.Exceptions;
using NetBlade.Core.Transaction;
using System;
using System.Threading.Tasks;

namespace NetBlade.Core.Commands.Test.Model.CommandHandlers
{
    public class SalvarPessoa2CommandHandler : CommandHandlerBase<SalvarPessoa2Command, SalvarPessoa2CommandResponse>
    {
        public SalvarPessoa2CommandHandler(ITransactionManager transactionManager, SalvarPessoa2Validation validation)
            : base(transactionManager, validation)
        {
        }

        protected override async Task<SalvarPessoa2CommandResponse> HandleAsync(SalvarPessoa2Command command)
        {
            if (command.Codigo <= -1)
            {
                throw new DomainException("Id nao permitido!");
            }

            SalvarPessoa2CommandResponse result = this.CommandResponseOk<SalvarPessoa2CommandResponse>();
            result.Contrato = Guid.NewGuid().ToString();
            result.Codigo = command.Codigo ?? 1;

            return await Task.Factory.StartNew(() => result);
        }
    }
}
