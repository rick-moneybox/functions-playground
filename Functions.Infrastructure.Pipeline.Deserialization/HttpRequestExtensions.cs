using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline.Deserialization
{
    public static class HttpRequestExtensions
    {
        public static async Task<(bool IsDeserialized, object Body)> TryDeserializeRequestBodyAsync(this HttpRequest request, Type bodyType)
        {
            var result = new object();

            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
                request.Body.Position = 0;

                result = JsonConvert.DeserializeObject(requestBody, bodyType);
                return (true, result);
            }
            catch
            {
                return (false, result);
            }
        }
    }
}
