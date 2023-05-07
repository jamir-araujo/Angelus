using System.Collections.Generic;

namespace Angelus.Receiveing
{
    internal class ReceiveContextBuilder : IReceiveContextBuilder
    {
        private readonly IEnumerable<IReceiveConvention> _conventions;
        private readonly IEnumerable<IConfigureReceive> _configures;

        public ReceiveContextBuilder(
            IEnumerable<IReceiveConvention> conventions,
            IEnumerable<IConfigureReceive> configures)
        {
            _conventions = conventions;
            _configures = configures;
        }

        public void Build(IReceiveContext context)
        {
            foreach (var convention in _conventions)
            {
                convention.Apply(context);
            }

            foreach (var configure in _configures)
            {
                configure.Configure(context);
            }
        }
    }
}
