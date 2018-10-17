using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastucture.Pipeline
{
    public interface IPipelineBehavior<TFunctionParams>
    {
        Task<HttpResponseMessage> Process(
            TFunctionParams @params,
            Func<TFunctionParams, Task<HttpResponseMessage>> next);
    }
}
