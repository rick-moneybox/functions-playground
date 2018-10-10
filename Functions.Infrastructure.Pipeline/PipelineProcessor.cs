using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public static class PipelineProcessor
    {
        private static readonly IPipelineBehavior[] _pipelineBehaviors;

        static PipelineProcessor()
        {
            // Order matters here
            _pipelineBehaviors = new IPipelineBehavior[]
            {
            };
        }

        public static async Task<IActionResult> ProcessAsJsonRequest(HttpRequest request, ILogger logger, IRequestHandler handler)
        {
            return await ProcessAsJsonRequest(request, logger, handler, 0);
        }

        static async Task<IActionResult> ProcessAsJsonRequest(
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

            return await pipelineBehavior.Process(request, logger, (r, l) => ProcessAsJsonRequest(r, l, handler, depth + 1));
        }
    }
}
