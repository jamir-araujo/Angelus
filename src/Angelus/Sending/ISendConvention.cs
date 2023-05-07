namespace Angelus.Sending
{
    public interface ISendConvention
    {
    }

    public interface ISendConvention<TMessage> : ISendConvention
    {
        void Apply(ISendContext<TMessage> context);
    }

    public abstract class SendConvention<TMessage> : ISendConvention<TMessage>
    {
        public abstract void Apply(ISendContext<TMessage> context);
    }
}
