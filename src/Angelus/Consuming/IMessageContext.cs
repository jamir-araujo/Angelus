using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus
{
    public interface IMessageContext<out TMessage>
    {
        string Source { get; }
        string Type { get; }
        string CorrelationId { get; }
        string MessageId { get; }
        string ContentType { get; }
        DateTimeOffset? Time { get; }
        IDictionary<string, object> Headers { get; }
        IDictionary<string, object> Items { get; }
        TMessage Message { get; }

        Task ComsumedAsync(CancellationToken cancellationToken = default);
        Task FailedAsync(Exception exception, CancellationToken cancellationToken = default);
    }
}
