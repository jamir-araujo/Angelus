using System;

namespace Angelus.Subscriptions
{
    public interface ISubscriptionHandler
    {
        void Unsubscribe();
    }

    internal class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly Action _unsubscribe;

        public SubscriptionHandler(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Unsubscribe() => _unsubscribe?.Invoke();
    }
}
