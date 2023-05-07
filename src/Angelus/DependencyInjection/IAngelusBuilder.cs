using System;

using Angelus;
using Angelus.Receiveing;
using Angelus.Sending;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAngelusBuilder
    {
        IServiceCollection Services { get; }

        IAngelusBuilder AddSendConvention<TConvention>() where TConvention : class;
        IAngelusBuilder AddSendConvention<TMessage>(Action<ISendContext<TMessage>> convention);

        IAngelusBuilder AddReceiveConvention(Action<IReceiveContext> convention);
        IAngelusBuilder AddReceiveConvention<TConvetion>() where TConvetion : class, IReceiveConvention;

        IAngelusBuilder ConfigureSend<TConfigureSend>() where TConfigureSend : class;
        IAngelusBuilder ConfigureSend<TMessage>(Action<ISendContext<TMessage>> configure);
        IAngelusBuilder ConfigureSend<TMessage>(string target);

        IAngelusBuilder ConfigureReceive<TConfigureReceive>() where TConfigureReceive : class, IConfigureReceive;
        IAngelusBuilder ConfigureReceive<TMessage>(Func<IReceiveContext, bool> when);
        IAngelusBuilder ConfigureReceive<TMessage>(string type, string source = null);

        TTransportBuilder GetOrAddTransport<TTransportBuilder>(string transportName, Func<string, TTransportBuilder> factory);
    }
}
