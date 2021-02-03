using MediatR;
using NetBlade.Core.Events;

namespace NetBlade.CrossCutting.MediatR.Messages
{
    public class MessageMediatR : INotification
    {
    }

    public class MessageMediatR<TMessage> : MessageMediatR
        where TMessage : Message
    {
        internal MessageMediatR(TMessage baseMessage)
        {
            this.BaseMessage = baseMessage;
        }

        internal TMessage BaseMessage { get; }
    }
}
