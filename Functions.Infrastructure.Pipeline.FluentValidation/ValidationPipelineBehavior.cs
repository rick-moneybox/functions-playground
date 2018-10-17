using Functions.Infrastucture.Pipeline;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public class ValidationPipelineBehavior<TFunctionParams> : IPipelineBehavior<TFunctionParams>
        where TFunctionParams : IValidatableFunctionParams
    {
        public async Task<HttpResponseMessage> Process(TFunctionParams @params, Func<TFunctionParams, Task<HttpResponseMessage>> next)
        {
            var requestBody = await @params.Request.DeserializeRequestBodyAsync(@params.BodyType);

            if (typeof(IValidatedRequest).IsAssignableFrom(@params.BodyType))
            {
                var validator = ((IValidatedRequest)requestBody).GetValidator();
                var validationResult = await validator.ValidateAsync(requestBody);

                if (!validationResult.IsValid)
                {
                    return new ErrorJsonResponse(System.Net.HttpStatusCode.BadRequest, $"{validationResult.Errors.First().ErrorMessage}");
                }
            }

            return await next(@params);
        }
    }
}
