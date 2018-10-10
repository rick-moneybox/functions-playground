using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public class ValidationPipelineBehavior : IPipelineBehavior
    {
        public async Task<HttpResponseMessage> Process<TRequest>(HttpRequest request, ILogger logger, Func<HttpRequest, ILogger, Task<HttpResponseMessage>> inner)
        {
            var requestBody = await request.DeserializeRequestBodyAsync<TRequest>();

            if (requestBody is IValidatedRequest)
            {
                var validator = ((IValidatedRequest)requestBody).GetValidator();
                var validationResult = await validator.ValidateAsync(requestBody);
                
                if (!validationResult.IsValid)
                {
                    return new ErrorJsonResponse(System.Net.HttpStatusCode.BadRequest, $"{validationResult.Errors.First().ErrorMessage}");
                }
            }

            // TODO:
            return await inner(request, logger);
        }
    }
}
