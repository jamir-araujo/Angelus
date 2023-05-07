using System.Threading;
using System.Threading.Tasks;

namespace Angelus
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync(IReceiveContext context, CancellationToken cancellationToken = default);
    }
}
