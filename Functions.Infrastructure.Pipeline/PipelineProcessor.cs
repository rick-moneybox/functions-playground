using Functions.Infrastucture.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public class PipelineProcessor
    {
        public static PipelineProcessorConfiguration<TFunctionParams> DefineFor<TFunctionParams>()
        {
            return new PipelineProcessorConfiguration<TFunctionParams>();
        }
    }

    public class PipelineProcessor<TFunctionParams>
    {
        readonly IPipelineBehavior<TFunctionParams>[] _pipelineBehaviors;

        internal PipelineProcessor(params IPipelineBehavior<TFunctionParams>[] pipelineBehaviors)
        {
            _pipelineBehaviors = pipelineBehaviors;
        }

        public Task<HttpResponseMessage> Process(
            TFunctionParams functionParams,
            IRequestHandler<TFunctionParams> handler)
        {
            return Process(functionParams, handler, 0);
        }

        async Task<HttpResponseMessage> Process(
            TFunctionParams functionParams,
            IRequestHandler<TFunctionParams> handler,
            int depth)
        {
            if (depth >= _pipelineBehaviors.Length)
            {
                return await handler.Handle(functionParams);
            }

            var pipelineBehavior = _pipelineBehaviors[depth];

            return await pipelineBehavior.Process(functionParams, p => Process(p, handler, depth + 1));
        }
    }

    public class PipelineProcessorConfiguration<TFunctionParams>
    {
        readonly Dictionary<int, IPipelineBehavior<TFunctionParams>> _pipelineBehaviors;

        internal PipelineProcessorConfiguration()
        {
            _pipelineBehaviors = new Dictionary<int, IPipelineBehavior<TFunctionParams>>();
        }

        public PipelineProcessorConfiguration<TFunctionParams> NextInPipeline(Func<IPipelineBehavior<TFunctionParams>> pipelineBehaviorInitializer)
        {
            _pipelineBehaviors.Add(_pipelineBehaviors.Count + 1, pipelineBehaviorInitializer());

            return this;
        }

        public PipelineProcessor<TFunctionParams> Build()
        {
            return new PipelineProcessor<TFunctionParams>(
                _pipelineBehaviors.AsEnumerable()
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToArray());
        }
    }
}
