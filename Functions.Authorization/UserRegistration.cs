using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure;
using Functions.Infrastructure.Pipeline;

namespace Functions.Authorization
{
    public static class UserRegistration
    {
        [FunctionName("UserRegistration")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, AllowedMethods.POST, Route = "users/register")]HttpRequest req, ILogger log)
        {
            return await PipelineProcessor.ProcessAsJsonRequest(req, log, new RequestHandler());
        }

        public class RequestHandler : IRequestHandler
        {
            public async Task<IActionResult> Handle(HttpRequest request, ILogger logger)
            {
                await Task.Yield();
                return new OkResult();
            }
        }
    }
}
