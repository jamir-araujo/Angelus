using System;
using System.Collections.Generic;

using Angelus;
using Angelus.Receiveing;
using Angelus.Sending;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AngelusBuilder : IAngelusBuilder
    {
        private readonly TransportBuilderCache _transportBuilderCache = new TransportBuilderCache();

        public AngelusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IAngelusBuilder AddReceiveConvention(Action<IReceiveContext> convention)
        {
            Services.AddSingleton<IReceiveConvention>(new RelayReceiveConvention(convention));

            return this;
        }

        public IAngelusBuilder ConfigureReceive<TMessage>(string type, string source = null)
        {
            return ConfigureReceive<TMessage>(context =>
            {
                if (type != context.Type)
                {
                    return false;
                }

                if (source == null || source != context.Source)
                {
                    return false;
                }

                return true;
            });
        }

        public IAngelusBuilder ConfigureReceive<TMessage>(Func<IReceiveContext, bool> when)
        {
            Services.AddSingleton<IReceiveConvention>(new RelayReceiveConvention(context =>
            {
                if (when(context))
                {
                    context.MatchTypes.Add(typeof(TMessage));
                }
            }));

            return this;
        }

        public IAngelusBuilder AddReceiveConvention<TConvetion>() where TConvetion : class, IReceiveConvention
        {
            Services.AddSingleton<IReceiveConvention, TConvetion>();

            return this;
        }

        public IAngelusBuilder AddSendConvention<TConvention>() where TConvention : class
        {
            var implementationType = typeof(TConvention);

            CheckRequiredOpenGenericInheritance(implementationType, typeof(SendConvention<>));

            Services.AddScoped(typeof(ISendConvention), implementationType);

            return this;
        }

        public IAngelusBuilder AddSendConvention<TMessage>(Action<ISendContext<TMessage>> convention)
        {
            Services.AddSingleton<ISendConvention>(new RelaySendConvention<TMessage>(convention));

            return this;
        }

        public IAngelusBuilder ConfigureSend<TMessage>(string target)
        {
            return ConfigureSend<TMessage>(context =>
            {
                context.Target = target;
            });
        }

        public IAngelusBuilder ConfigureSend<TMessage>(Action<ISendContext<TMessage>> configure)
        {
            Services.AddSingleton<IConfigureSend>(new RelayConfigureSend<TMessage>(configure));

            return this;
        }

        public TTransportBuilder GetOrAddTransport<TTransportBuilder>(string transportName, Func<string, TTransportBuilder> factory)
        {
            return _transportBuilderCache.GetOrAdd(transportName, factory);
        }

        public IAngelusBuilder ConfigureSend<TConfigureSend>() where TConfigureSend : class
        {
            var implementationType = typeof(TConfigureSend);

            CheckRequiredOpenGenericInheritance(implementationType, typeof(ConfigureSend<>));

            Services.AddScoped(typeof(IConfigureSend), implementationType);

            return this;
        }

        public IAngelusBuilder ConfigureReceive<TConfigureReceive>() where TConfigureReceive : class, IConfigureReceive
        {
            Services.AddSingleton<IConfigureReceive, TConfigureReceive>();

            return this;
        }

        private void CheckRequiredOpenGenericInheritance(Type implementationType, Type requiredInheritance)
        {
            var baseType = implementationType.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == requiredInheritance)
                {
                    return;
                }

                baseType = baseType.BaseType;
            }

            throw new InvalidOperationException($"{implementationType} must inherit from {requiredInheritance} to be registered");
        }

        class TransportBuilderCache
        {
            private readonly Dictionary<Type, TransportTypeCache> _transportTypeCaches = new Dictionary<Type, TransportTypeCache>();

            public TTransportBuilder GetOrAdd<TTransportBuilder>(string transportName, Func<string, TTransportBuilder> factory)
            {
                var transportTypeCache = _transportTypeCaches.GetOrAdd(typeof(TTransportBuilder), t => new TransportTypeCache(t));

                return transportTypeCache.GetOrAdd(transportName, factory);
            }
        }

        class TransportTypeCache
        {
            private readonly Dictionary<string, object> _transports = new Dictionary<string, object>();

            public TransportTypeCache(Type transportType)
            {
                TransportType = transportType;
            }

            public Type TransportType { get; set; }

            public TTransportBuilder GetOrAdd<TTransportBuilder>(string transportName, Func<string, TTransportBuilder> factory)
            {
                return (TTransportBuilder)_transports.GetOrAdd(transportName, n => factory(n));
            }
        }
    }
}
