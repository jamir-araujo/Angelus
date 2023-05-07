using System;

namespace Angelus.Receiveing
{
    internal class RelayReceiveConvention : IReceiveConvention
    {
        private readonly Action<IReceiveContext> _convention;

        public RelayReceiveConvention(Action<IReceiveContext> convention) => _convention = convention;

        public void Apply(IReceiveContext context) => _convention?.Invoke(context);
    }
}
