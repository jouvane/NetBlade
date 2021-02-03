using NetBlade.Core.Domain.Test.Events.Model.Entitys;
using NetBlade.Core.EventContexts;
using NetBlade.Core.Mediator;
using NetBlade.Core.Services;
using System.Threading.Tasks;

namespace NetBlade.Core.Domain.Test.Events.Model.Services
{
    public class ProdutoServices : ServiceBase
    {
        public ProdutoServices(EventContext eventContext, IMediatorHandler mediatorHandler)
            : base(eventContext, mediatorHandler)
        {
        }

        public async Task AtualizarValorUnitario(int codigo, double valor)
        {
            ProdutoModel p = new ProdutoModel
            {
                ID = codigo,
                Nome = "Sabao",
                ValorUnitario = 100
            };

            p.AtualizarValorUnitario(valor);
            await this.RaiseEvents(p);
        }
    }
}
