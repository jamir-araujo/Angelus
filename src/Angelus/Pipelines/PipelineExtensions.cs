using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Angelus.Pipelines
{
    public static class PipelineExtensions
    {
        public static Task SendAsync<TContext>(this IPipeline<TContext> pipeline, TContext context, CancellationToken cancellationToken = default)
            => pipeline.SendAsync(context, () => Task.CompletedTask, cancellationToken);

        public static IPipeline<TContext> Append<TContext>(this IPipeline<TContext> pipeline, params IMiddleware<TContext>[] next)
            => Pipeline.Append(pipeline, next);

        public static IPipeline<TContext> Append<TContext>(this IPipeline<TContext> pipeline, IEnumerable<IMiddleware<TContext>> next)
            => Pipeline.Append(pipeline, next);

        public static IPipeline<TContext> Append<TContext>(this IPipeline<TContext> pipeline, params Middleware<TContext>[] next)
            => Pipeline.Append(pipeline, next);

        public static IPipeline<TContext> Append<TContext>(this IPipeline<TContext> pipeline, IEnumerable<Middleware<TContext>> next)
            => Pipeline.Append(pipeline, next);

        public static IPipeline<TContext> Prepend<TContext>(this IPipeline<TContext> pipeline, params IMiddleware<TContext>[] next)
            => Pipeline.Prepend(pipeline, next);

        public static IPipeline<TContext> Prepend<TContext>(this IPipeline<TContext> pipeline, IEnumerable<IMiddleware<TContext>> next)
            => Pipeline.Prepend(pipeline, next);

        public static IPipeline<TContext> Prepend<TContext>(this IPipeline<TContext> pipeline, params Middleware<TContext>[] next)
            => Pipeline.Prepend(pipeline, next);

        public static IPipeline<TContext> Prepend<TContext>(this IPipeline<TContext> pipeline, IEnumerable<Middleware<TContext>> next)
            => Pipeline.Prepend(pipeline, next);

        public static IPipeline<TContext> Concat<TContext>(this IPipeline<TContext> pipeline, IPipeline<TContext> next)
            => Pipeline.Concat(pipeline, next);
    }
}
