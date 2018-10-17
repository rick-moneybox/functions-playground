using Functions.Infrastucture.Pipeline;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public class FluentValidationPipelineBehavior<TFunctionParams> : IPipelineBehavior<TFunctionParams>
        where TFunctionParams : IValidatableFunctionParams
    {
        public async Task<HttpResponseMessage> Process(TFunctionParams @params, Func<TFunctionParams, Task<HttpResponseMessage>> next)
        {
            var requestBody = await @params.Request.DeserializeRequestBodyAsync(@params.BodyType);

            if (typeof(IFluentValidatedRequest).IsAssignableFrom(@params.BodyType))
            {
                var validator = ((IFluentValidatedRequest)requestBody).GetValidator();
                var validationResult = await validator.ValidateAsync(requestBody);

                if (!validationResult.IsValid)
                {
                    return new ErrorJsonResponse(System.Net.HttpStatusCode.BadRequest, $"{validationResult.Errors.First().ErrorMessage}");
                }

                return await next(@params);
            }

            throw new NotImplementedException(
                $"Expected a validated request but the request body has no validator. Ensure that '{@params.BodyType.Name}' implements IFluentValidatedRequest");
        }
    }
}
