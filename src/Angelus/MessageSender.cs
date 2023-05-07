using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Angelus.Sending;
using Angelus.Transport;

namespace Angelus
{
    public class MessageSender : IMessageSender
    {
        private readonly ISendContextBuilder _builder;
        private readonly IEnumerable<ITransportSender> _senders;

        public MessageSender(
            ISendContextBuilder builder,
            IEnumerable<ITransportSender> senders)
        {
            _builder = Checks.NotNull(builder, nameof(builder));
            _senders = Checks.NotNull(senders, nameof(senders));
        }

        public async Task SendAsync<TMessage>(TMessage message, Func<ISendContext<TMessage>, Task> contextBuilder, CancellationToken cancellationToken = default)
        {
            var context = new SendContext<TMessage>(message);

            _builder.Build(context);

            if (contextBuilder != null)
            {
                await contextBuilder(context);
            }

            foreach (var sender in _senders)
            {
                await sender.SendAsync(context, cancellationToken);
            }
        }
    }
}
