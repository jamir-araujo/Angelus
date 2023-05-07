using System;

namespace Angelus.Sending
{
    internal class RelaySendConvention<TMessage> : ISendConvention<TMessage>
    {
        private readonly Action<ISendContext<TMessage>> _convention;

        public RelaySendConvention(Action<ISendContext<TMessage>> convention) => _convention = convention;

        public void Apply(ISendContext<TMessage> context) => _convention?.Invoke(context);
    }
}
