using System.Threading;
using System.Threading.Tasks;

namespace NetBlade.Core.Events
{
    public interface IEventHandler<in T>
        where T : Message
    {
        Task Handle(T notification, CancellationToken cancellationToken);
    }
}
