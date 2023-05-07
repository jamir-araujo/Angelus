using System;
using System.Collections.Generic;

using Angelus.Serializers;

namespace Angelus
{
    public interface ISendContext<out TMessage>
    {
        string Target { get; set; }
        string Type { get; set; }
        string CorrelationId { get; set; }
        string MessageId { get; set; }
        string ContentType { get; set; }
        DateTimeOffset? Time { get; set; }
        IDictionary<string, object> Headers { get; }
        IDictionary<string, object> Items { get; }
        IMessageSerializer Serializer { get; set; }
        TMessage Message { get; }
    }

    public class SendContext<TMessage> : ISendContext<TMessage>
    {
        public SendContext(TMessage message)
        {
            Message = message;
        }

        public string Target { get; set; }
        public string Type { get; set; }
        public string CorrelationId { get; set; }
        public string MessageId { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset? Time { get; set; }
        public IDictionary<string, object> Headers { get; }
        public IDictionary<string, object> Items { get; }
        public IMessageSerializer Serializer { get; set; }
        public TMessage Message { get; }
    }
}
