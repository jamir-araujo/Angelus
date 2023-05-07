using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Angelus.Serializers;

namespace Angelus
{
    public interface IReceiveContext
    {
        string Source { get; }
        string Type { get; }
        string CorrelationId { get; }
        string MessageId { get; }
        string ContentType { get; }
        DateTimeOffset? Time { get; }
        IDictionary<string, object> Headers { get; }
        IDictionary<string, object> Items { get; }
        HashSet<Type> MatchTypes { get; }
        IMessageSerializer Serializer { get; set; }
        ReadOnlyMemory<byte> GetBody();
        int ConsumeCount { get; }

        Task ComsumedAsync(CancellationToken cancellationToken = default);
        Task FailedAsync(Exception exception, CancellationToken cancellationToken = default);
    }
}
