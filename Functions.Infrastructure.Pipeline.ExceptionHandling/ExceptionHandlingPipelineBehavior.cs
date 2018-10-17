using Functions.Infrastucture.Pipeline;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.ExceptionHandling
{
    public class ExceptionHandlingPipelineBehavior<TFunctionParams> : IPipelineBehavior<TFunctionParams>
    {
        public async Task<HttpResponseMessage> Process(TFunctionParams @params, Func<TFunctionParams, Task<HttpResponseMessage>> next)
        {
            try
            {
                return await next(@params);
            }
            catch (Exception ex)
            {
                return new ErrorJsonResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
