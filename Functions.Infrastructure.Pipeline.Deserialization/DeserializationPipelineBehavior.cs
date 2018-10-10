using Functions.Infrastucture.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.Deserialization
{
    public class DeserializationPipelineBehavior : IPipelineBehavior
    {
        public async Task<HttpResponseMessage> Process<TRequest>(HttpRequest request, ILogger logger, Func<HttpRequest, ILogger, Task<HttpResponseMessage>> inner)
        {
            var (IsDeserialized, Body) = await request.TryDeserializeRequestBodyAsync<TRequest>();

            if (!IsDeserialized)
            {
                var result = new ErrorJsonResponse(HttpStatusCode.BadRequest, "Invalid JSON Body")
                {
                    StatusCode = HttpStatusCode.BadRequest
                };

                return result;
            }

            return await inner(request, logger);
        }
    }
}
