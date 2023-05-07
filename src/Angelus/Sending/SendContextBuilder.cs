using System.Collections.Generic;

namespace Angelus.Sending
{
    public class SendContextBuilder : ISendContextBuilder
    {
        private readonly IEnumerable<ISendConvention> _conventions;
        private readonly IEnumerable<IConfigureSend> _configures;

        public SendContextBuilder(
            IEnumerable<ISendConvention> conventions,
            IEnumerable<IConfigureSend> configures)
        {
            _conventions = conventions;
            _configures = configures;
        }

        public void Build<TMessage>(ISendContext<TMessage> context)
        {
            foreach (var convention in _conventions)
            {
                if (convention is ISendConvention<TMessage> messageConvention)
                {
                    messageConvention.Apply(context);
                }
            }

            foreach (var configure in _configures)
            {
                if (configure is IConfigureSend<TMessage> messageConfigure)
                {
                    messageConfigure.Configure(context);
                }
            }
        }
    }
}
