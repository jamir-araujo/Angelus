using System;

namespace Angelus.Receiveing
{
    internal class RelayConfigureReceive : IConfigureReceive
    {
        private readonly Action<IReceiveContext> _configure;

        public RelayConfigureReceive(Action<IReceiveContext> configure) => _configure = configure;

        public void Configure(IReceiveContext context) => _configure?.Invoke(context);
    }
}
