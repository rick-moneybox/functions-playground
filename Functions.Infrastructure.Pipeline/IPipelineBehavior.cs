using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastucture.Pipeline
{
    public interface IPipelineBehavior
    {
        Task<HttpResponseMessage> Process<TRequest>(
            HttpRequest request, 
            ILogger logger, 
            Func<HttpRequest, ILogger, Task<HttpResponseMessage>> inner);
    }
}
