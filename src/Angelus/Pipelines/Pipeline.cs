using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Pipelines
{
    public class Pipeline
    {
        public static IPipeline<TContext> Create<TContext>(IEnumerable<IMiddleware<TContext>> middlewares)
        {
            if (middlewares.Any())
            {
                var middleware = middlewares.Aggregate((outer, inner) => new MiddlewareConnector<TContext>(outer, inner));
                return new Pipeline<TContext>(middleware);
            }
            else
            {
                var middleware = new RelayMiddleware<TContext>(async (c, n, ct) => await n());
                return new Pipeline<TContext>(middleware);
            }
        }

        public static IPipeline<TContext> Create<TContext>(params Middleware<TContext>[] middlewares)
            => Create(middlewares.AsEnumerable());

        public static IPipeline<TContext> Create<TContext>(IEnumerable<Middleware<TContext>> middlewares)
            => Create(middlewares.Select(m => new RelayMiddleware<TContext>(m)));

        public static IPipeline<TContext> Append<TContext>(IPipeline<TContext> pipeline, params IMiddleware<TContext>[] next)
            => Append(pipeline, next.AsEnumerable());

        public static IPipeline<TContext> Append<TContext>(IPipeline<TContext> pipeline, IEnumerable<IMiddleware<TContext>> next)
            => Concat(pipeline, Create(next));

        public static IPipeline<TContext> Append<TContext>(IPipeline<TContext> pipeline, params Middleware<TContext>[] next)
            => Append(pipeline, next.AsEnumerable());

        public static IPipeline<TContext> Append<TContext>(IPipeline<TContext> pipeline, IEnumerable<Middleware<TContext>> next)
            => Concat(pipeline, Create(next));

        public static IPipeline<TContext> Prepend<TContext>(IPipeline<TContext> pipeline, params IMiddleware<TContext>[] next)
            => Prepend(pipeline, next.AsEnumerable());

        public static IPipeline<TContext> Prepend<TContext>(IPipeline<TContext> pipeline, IEnumerable<IMiddleware<TContext>> next)
            => Concat(Create(next), pipeline);

        public static IPipeline<TContext> Prepend<TContext>(IPipeline<TContext> pipeline, params Middleware<TContext>[] next)
            => Prepend(pipeline, next.AsEnumerable());

        public static IPipeline<TContext> Prepend<TContext>(IPipeline<TContext> pipeline, IEnumerable<Middleware<TContext>> next)
            => Concat(Create(next), pipeline);

        public static IPipeline<TContext> Concat<TContext>(IPipeline<TContext> pipeline, IPipeline<TContext> next)
            => new PipelineConnector<TContext>(pipeline, next);

        class PipelineConnector<TContext> : IPipeline<TContext>
        {
            private readonly IPipeline<TContext> _inner;
            private readonly IPipeline<TContext> _outer;

            public PipelineConnector(IPipeline<TContext> inner, IPipeline<TContext> outer)
            {
                _inner = inner;
                _outer = outer;
            }

            public async Task SendAsync(TContext context, Func<Task> next, CancellationToken cancellationToken = default)
            {
                await _inner.SendAsync(context, async () =>
                {
                    await _outer.SendAsync(context, next, cancellationToken);
                }, cancellationToken);
            }
        }

        class MiddlewareConnector<TContext> : IMiddleware<TContext>
        {
            private readonly IMiddleware<TContext> _outer;
            private readonly IMiddleware<TContext> _inner;

            public MiddlewareConnector(IMiddleware<TContext> outer, IMiddleware<TContext> inner)
            {
                _outer = outer;
                _inner = inner;
            }

            public async Task InvokeAsync(TContext context, Func<Task> next, CancellationToken cancellationToken = default)
                => await _outer.InvokeAsync(context, async () => await _inner.InvokeAsync(context, next, cancellationToken), cancellationToken);
        }

        class RelayMiddleware<TContext> : IMiddleware<TContext>
        {
            private readonly Middleware<TContext> _middleware;

            public RelayMiddleware(Middleware<TContext> middleware)
                => _middleware = middleware;

            public async Task InvokeAsync(TContext context, Func<Task> next, CancellationToken cancellationToken = default)
                => await _middleware(context, next, cancellationToken);
        }
    }

    internal class Pipeline<TContext> : IPipeline<TContext>
    {
        private readonly IMiddleware<TContext> _middleware;

        public Pipeline(IMiddleware<TContext> middleware)
            => _middleware = middleware;

        public async Task SendAsync(TContext context, Func<Task> callback, CancellationToken cancellationToken = default)
            => await _middleware.InvokeAsync(context, callback, cancellationToken);
    }
}
