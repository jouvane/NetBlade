using NetBlade.Core.Transaction;
using System;

namespace NetBlade.Core.Commands.Test.Model.Commands
{
    public class SalvarPessoaRequiresNewTransactionScopeCommand : Command, IRequiresNewTransactionScope
    {
        public Action<ITransactionManager> Valid { get; set; }
    }
}
