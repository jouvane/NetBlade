using NetBlade.Core.Domain.Test.Events.Model.DomainEvents;
using NetBlade.Core.Domain.Test.Events.Model.Entitys;
using NetBlade.Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Core.Domain.Test.Events.Model.EventHandlers
{
    public class PromocaoEmailEventHandler : IEventHandler<AtualizarValorUnitarioEvent>
    {
        public const string MSG = "Para Confira a promocao {0} de {1} por {2} --> geovane.simoes@squadra.com.br";
        private readonly ISendMsgTest _sendMsgTest;

        public PromocaoEmailEventHandler(ISendMsgTest sendMsgTest)
        {
            this._sendMsgTest = sendMsgTest;
        }

        public async Task Handle(AtualizarValorUnitarioEvent notification, CancellationToken cancellationToken)
        {
            ProdutoModel p = notification.EntityAs<ProdutoModel>();
            await Task.Delay(700, cancellationToken);

            if (p != null && p.ValorUnitario < notification.OldValue)
            {
                this.EnviarMail(string.Format(PromocaoEmailEventHandler.MSG, p.Nome, notification.OldValue, p.ValorUnitario));
            }
        }

        private void EnviarMail(string msg)
        {
            this._sendMsgTest.SendMsg(msg);
        }
    }
}
