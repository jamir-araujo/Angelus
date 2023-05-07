using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KafkaServiceCollectionExtensions
    {
        public static IAngelusBuilder AddKafkaTransport(this IAngelusBuilder builder, Action<IKafkaTransportBuilder> configure)
        {
            return builder;
        }
    }
}
