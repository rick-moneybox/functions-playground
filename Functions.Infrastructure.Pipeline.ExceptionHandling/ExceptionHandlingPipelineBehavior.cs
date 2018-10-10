using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.ExceptionHandling
{
    public class ExceptionHandlingPipelineBehavior : IPipelineBehavior
    {
        public async Task<HttpResponseMessage> Process<TRequest>(HttpRequest request, ILogger logger, Func<HttpRequest, ILogger, Task<HttpResponseMessage>> inner)
        {
            try
            {
                return await inner(request, logger);
            }
            catch (Exception ex)
            {
                return new ErrorJsonResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
