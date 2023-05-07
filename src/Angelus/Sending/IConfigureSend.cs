namespace Angelus.Sending
{
    public interface IConfigureSend
    {
    }

    public interface IConfigureSend<TMessage> : IConfigureSend
    {
        void Configure(ISendContext<TMessage> context);
    }

    public abstract class ConfigureSend<TMessage> : IConfigureSend<TMessage>
    {
        public abstract void Configure(ISendContext<TMessage> context);
    }
}
