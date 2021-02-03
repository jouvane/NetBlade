using System.Threading.Tasks;

namespace NetBlade.Core.Transaction
{
    public delegate Task<TResponse> TransactionHandlerDelegate<TResponse>();
}
