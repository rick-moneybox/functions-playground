using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Functions.Infrastucture.Pipeline
{
    public interface IPipelineBehavior
    {
        Task<IActionResult> Process(
            HttpRequest request, 
            ILogger logger, 
            Func<HttpRequest, ILogger, Task<IActionResult>> inner);
    }
}
