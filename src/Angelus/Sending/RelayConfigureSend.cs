using System;

namespace Angelus.Sending
{
    internal class RelayConfigureSend<TMessage> : IConfigureSend<TMessage>
    {
        private readonly Action<ISendContext<TMessage>> _configure;

        public RelayConfigureSend(Action<ISendContext<TMessage>> configure) => _configure = configure;

        public void Configure(ISendContext<TMessage> context) => _configure?.Invoke(context);
    }
}
