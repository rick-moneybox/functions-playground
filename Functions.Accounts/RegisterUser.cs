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
using Microsoft.Azure.Documents.Client;
using Functions.Accounts.Core.Repositories;
using Functions.Accounts.DataAccess.Repositories;
using Functions.Accounts.Core.Domain;

namespace Functions.Accounts
{
    public class FunctionParams : IJsonDeserializableHttpFunctionParams, IValidatableFunctionParams
    {
        public Type BodyType { get; set; }

        public HttpRequest Request { get; set; }

        public DocumentClient UsersDocumentClient { get; set; }
    }

    public static class RegisterUser
    {
        public static PipelineProcessor<FunctionParams> _pipelineProcessor;

        static RegisterUser()
        {
            _pipelineProcessor = PipelineProcessor
                .DefineFor<FunctionParams>()
                .NextInPipeline(() => new ExceptionHandlingPipelineBehavior<FunctionParams>())
                .NextInPipeline(() => new JsonDeserializationPipelineBehavior<FunctionParams>())
                .NextInPipeline(() => new FluentValidationPipelineBehavior<FunctionParams>())
                .Build();
        }

        [FunctionName("RegisterUser")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, AllowedMethods.POST, Route = "users/register")]HttpRequest req,
            [CosmosDB(
                databaseName: "Authorization",
                collectionName: "Users",
                ConnectionStringSetting = "CosmosDBConnection",
                CreateIfNotExists = true)] DocumentClient client,
            ILogger log)
        {
            var functionParams = new FunctionParams
            {
                Request = req,
                BodyType = typeof(Request),
                UsersDocumentClient = client
            };

            return await _pipelineProcessor.Process(functionParams, new RequestHandler());
        }

        public class Request : IFluentValidatedRequest
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

        public class CreatedResponse : JsonResponse
        {
            public CreatedResponse(string id)
                : base(HttpStatusCode.Created, new { Id = id })
            {
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
                var request = await @params.Request.DeserializeRequestBodyAsync<Request>();
                IUserRepository repository = new UserRepository(@params.UsersDocumentClient);

                var user = await repository.GetByEmailAsync(request.Email);
                if (user != null)
                {
                    return new ErrorJsonResponse(HttpStatusCode.BadRequest, "Email address already in use");
                }

                user = new User(request.Name, request.Email, request.Password);

                var id = await repository.InsertUserAsync(user);

                return new CreatedResponse(id);
            }
        }
    }
}
