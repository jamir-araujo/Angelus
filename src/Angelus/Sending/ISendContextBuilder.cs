using System.Collections;

namespace Angelus.Sending
{
    public interface ISendContextBuilder
    {
        void Build<TMessage>(ISendContext<TMessage> context);
    }
}
