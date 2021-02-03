using NetBlade.Core.Domain.Test.Events.Model.DomainEvents;
using NetBlade.Core.Domain.Test.Events.Model.Entitys;
using NetBlade.Core.Events;
using NetBlade.Core.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Core.Domain.Test.Events.Model.EventHandlers
{
    public class MensagemChatEventHandler : IEventHandler<AtualizarValorUnitarioEvent>
    {
        public const string MSG = "Confira a promocao {0} de {1} por {2}";
        private readonly ISendMsgTest _sendMsgTest;

        public MensagemChatEventHandler(IMediatorHandler mediatorHandler, ISendMsgTest sendMsgTest)
        {
            this._sendMsgTest = sendMsgTest;
        }

        public async Task Handle(AtualizarValorUnitarioEvent notification, CancellationToken cancellationToken)
        {
            ProdutoModel p = notification.EntityAs<ProdutoModel>();
            await Task.Delay(500, cancellationToken);

            if (p != null && p.ValorUnitario < notification.OldValue)
            {
                this.EnviarMensagemChatService(string.Format(MensagemChatEventHandler.MSG, p.Nome, notification.OldValue, p.ValorUnitario));
            }
        }

        private void EnviarMensagemChatService(string msg)
        {
            this._sendMsgTest.SendMsg(msg);
        }
    }
}
