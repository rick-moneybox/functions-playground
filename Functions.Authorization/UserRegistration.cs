using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure;
using Functions.Infrastructure.Pipeline;
using Functions.Infrastructure.Pipeline.Deserialization;
using Functions.Infrastructure.Pipeline.FluentValidation;
using System.Net.Http;
using System;
using FluentValidation;
using Functions.Infrastructure.Responses;
using System.Net;
using Functions.Infrastructure.Pipeline.ExceptionHandling;

namespace Functions.Authorization
{
    public static class UserRegistration
    {
        public static PipelineProcessor<FunctionParams> _pipelineProcessor;

        static UserRegistration()
        {
            _pipelineProcessor = PipelineProcessor
                .DefineFor<FunctionParams>()
                .NextInPipeline(() => new ExceptionHandlingPipelineBehavior<FunctionParams>())
                .NextInPipeline(() => new DeserializationPipelineBehavior<FunctionParams>())
                .NextInPipeline(() => new ValidationPipelineBehavior<FunctionParams>())
                .Build();
        }

        [FunctionName("UserRegistration")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, AllowedMethods.POST, Route = "users/register")]HttpRequest req, ILogger log)
        {
            var functionParams = new FunctionParams
            {
                Request = req,
                BodyType = typeof(Request)
            };

            return await _pipelineProcessor.Process(functionParams, new RequestHandler());
        }

        public class FunctionParams : IDeserializableHttpFunctionParams, IValidatableFunctionParams
        {
            public Type BodyType { get; set; }

            public HttpRequest Request { get; set; }
        }

        public class Request : IValidatedRequest
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string ConfirmPassword { get; set; }

            public IValidator GetValidator()
            {
                return new RequestValidator();
            }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required");
            }
        }

        public class RequestHandler : IRequestHandler<FunctionParams>
        {
            public async Task<HttpResponseMessage> Handle(FunctionParams @params)
            {
                await Task.Yield();
                return new JsonResponse(HttpStatusCode.OK, new object());
            }
        }
    }
}
