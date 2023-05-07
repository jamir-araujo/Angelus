using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Subscriptions
{
    public interface IMessageSubscriber
    {
        IEnumerable<ISubscription> GetSubscriptions();
        IEnumerable<ISubscription<TMessage>> GetSubscriptions<TMessage>() where TMessage : class;
        ISubscriptionHandler Subscribe<TMessage>(Func<IServiceProvider, IMessageContext<TMessage>, CancellationToken, Task> subcription) where TMessage : class;
        void Unsubscribe(ISubscription subscription);
    }

    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly List<ISubscription> _subscriptions = new List<ISubscription>();

        public IEnumerable<ISubscription> GetSubscriptions() => _subscriptions.AsReadOnly();

        public IEnumerable<ISubscription<TMessage>> GetSubscriptions<TMessage>() where TMessage : class
        {
            foreach (var subscription in _subscriptions)
            {
                if (subscription is ISubscription<TMessage> sub)
                {
                    yield return sub;
                }
            }
        }

        public ISubscriptionHandler Subscribe<TMessage>(Func<IServiceProvider, IMessageContext<TMessage>, CancellationToken, Task> subcription)
            where TMessage : class
        {
            var subscrition = new Subscription<TMessage>(subcription);

            _subscriptions.Add(subscrition);

            return new SubscriptionHandler(() =>
            {
                _subscriptions.Remove(subscrition);
            });
        }

        public void Unsubscribe(ISubscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}
