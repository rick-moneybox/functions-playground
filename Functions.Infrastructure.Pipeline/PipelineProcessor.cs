using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public class PipelineProcessor
    {
        readonly IPipelineBehavior[] _pipelineBehaviors;

        PipelineProcessor(params IPipelineBehavior[] pipelineBehaviors)
        {
            _pipelineBehaviors = pipelineBehaviors;
        }

        public static PipelineProcessor Build(params IPipelineBehavior[] pipelineBehaviors)
        {
            return new PipelineProcessor(pipelineBehaviors);
        }

        public async Task<HttpResponseMessage> ProcessAsJsonRequest<TRequest>(HttpRequest request, ILogger logger, IRequestHandler handler)
        {
            return await ProcessAsJsonRequest<TRequest>(request, logger, handler, 0);
        }

        async Task<HttpResponseMessage> ProcessAsJsonRequest<TRequest>(
            HttpRequest request, 
            ILogger logger,
            IRequestHandler handler,
            int depth)
        {
            if (depth >= _pipelineBehaviors.Length)
            {
                return await handler.Handle(request, logger);
            }

            var pipelineBehavior = _pipelineBehaviors[depth];

            return await pipelineBehavior.Process<TRequest>(request, logger, (r, l) => ProcessAsJsonRequest<TRequest>(r, l, handler, depth + 1));
        }
    }
}
