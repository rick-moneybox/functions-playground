using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public static class HttpRequestExtensions
    {
        public static async Task<object> DeserializeRequestBodyAsync(this HttpRequest request, Type bodyType)
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            return JsonConvert.DeserializeObject(requestBody, bodyType);
        }
    }
}
