using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Transport
{
    public interface ITransportListener
    {
        string TransportName { get; }

        Task StartListenAsync(CancellationToken cancellationToken = default);
        Task StopListenAsync(CancellationToken cancellationToken = default);
    }
}
