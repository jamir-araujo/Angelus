using System;

using Angelus.RabbitMq.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMqServiceCollectionExtensions
    {
        public static IAngelusBuilder AddRabbitMqTransport(this IAngelusBuilder builder, Action<IRabbitMqTransportBuilder> configure)
        {
            return builder;
        }
    }
}
