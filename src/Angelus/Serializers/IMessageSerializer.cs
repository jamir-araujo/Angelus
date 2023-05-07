using System;

namespace Angelus.Serializers
{
    public interface IMessageSerializer
    {
        bool CanDeserialize(IReceiveContext context);
        ReadOnlyMemory<byte> Serializer<TMessage>(ISendContext<TMessage> context);
        TMessage Deserialize<TMessage>(IReceiveContext context);
    }
}
