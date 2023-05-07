using System;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Subscriptions
{
    public interface ISubscription
    {
        Type MessageType { get; }
    }

    public interface ISubscription<TMessage> : ISubscription
    {
        Task InvokeAsync(IServiceProvider serviceProvider, IMessageContext<TMessage> context, CancellationToken cancellationToken = default);
    }

    internal class Subscription<TMessage> : ISubscription<TMessage> where TMessage : class
    {
        private readonly Func<IServiceProvider, IMessageContext<TMessage>, CancellationToken, Task> _handler;

        public Subscription(Func<IServiceProvider, IMessageContext<TMessage>, CancellationToken, Task> handler)
        {
            _handler = handler;
        }

        public Type MessageType => typeof(TMessage);

        public async Task InvokeAsync(IServiceProvider serviceProvider, IMessageContext<TMessage> context, CancellationToken cancellationToken = default)
        {
            await _handler(serviceProvider, context, cancellationToken);
        }
    }
}
