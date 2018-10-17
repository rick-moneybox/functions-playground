using Functions.Infrastucture.Pipeline;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.Deserialization
{
    public class JsonDeserializationPipelineBehavior<TFunctionParams> : IPipelineBehavior<TFunctionParams> 
        where TFunctionParams : IJsonDeserializableHttpFunctionParams
    {
        public async Task<HttpResponseMessage> Process(TFunctionParams @params, Func<TFunctionParams, Task<HttpResponseMessage>> next)
        {
            var (IsDeserialized, Body) = await @params.Request.TryDeserializeRequestBodyAsync(@params.BodyType);

            if (!IsDeserialized)
            {
                var result = new ErrorJsonResponse(HttpStatusCode.BadRequest, "Invalid JSON Body")
                {
                    StatusCode = HttpStatusCode.BadRequest
                };

                return result;
            }

            return await next(@params);
        }
    }
}
