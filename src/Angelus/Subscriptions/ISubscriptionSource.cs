namespace Angelus.Subscriptions
{
    public interface ISubscriptionSource
    {
        void Subscribe(IMessageSubscriber subscriber);
        void Unsubscribe();
    }
}
