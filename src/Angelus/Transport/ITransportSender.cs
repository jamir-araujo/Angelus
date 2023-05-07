using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Transport
{
    public interface ITransportSender
    {
        string TransportName { get; }

        Task SendAsync<TMessage>(ISendContext<TMessage> context, CancellationToken cancellationToken = default);
    }
}
