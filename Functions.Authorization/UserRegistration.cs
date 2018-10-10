using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure;
using Functions.Infrastructure.Pipeline;
using Functions.Infrastructure.Pipeline.Deserialization;
using Functions.Infrastructure.Pipeline.FluentValidation;
using System.Net.Http;
using System.Net;
using System;
using Functions.Infrastructure.Responses;

namespace Functions.Authorization
{
    public static class UserRegistration
    {
        public static PipelineProcessor _pipelineProcessor;

        static UserRegistration()
        {
            _pipelineProcessor = PipelineProcessor.Build(
                new DeserializationPipelineBehavior(),
                new ValidationPipelineBehavior());
        }

        [FunctionName("UserRegistration")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, AllowedMethods.POST, Route = "users/register")]HttpRequest req, ILogger log)
        {
            return await _pipelineProcessor.ProcessAsJsonRequest<Request>(req, log, new RequestHandler());
        }

        public class Request
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string ConfirmPassword { get; set; }
        }

        public class RequestHandler : IRequestHandler
        {
            public async Task<HttpResponseMessage> Handle(HttpRequest request, ILogger logger)
            {
                await Task.Yield();
                return new JsonResponse(HttpStatusCode.OK, new object());
            }
        }
    }
}
