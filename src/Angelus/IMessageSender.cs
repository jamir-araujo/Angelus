using System;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus
{
    public interface IMessageSender
    {
        Task SendAsync<TMessage>(TMessage message, Func<ISendContext<TMessage>, Task> contextBuilder, CancellationToken cancellationToken = default);
    }
}
