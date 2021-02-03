using System.Threading.Tasks;

namespace NetBlade.Scheduling
{
    public interface IJob<TData> : IJob
        where TData : new()
    {
        Task Execute(TData data);
    }
}