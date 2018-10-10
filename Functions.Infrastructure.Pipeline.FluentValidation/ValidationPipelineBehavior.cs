using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public class ValidationPipelineBehavior : IPipelineBehavior
    {
        public async Task<HttpResponseMessage> Process<TRequest>(HttpRequest request, ILogger logger, Func<HttpRequest, ILogger, Task<HttpResponseMessage>> inner)
        {
            // TODO:
            return await inner(request, logger);
        }
    }
}
