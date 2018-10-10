using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Functions.Infrastructure.Responses
{
    public class JsonResponse : HttpResponseMessage
    {
        public JsonResponse(HttpStatusCode statusCode, object response)
            : base(statusCode)
        {
            Content = new StringContent(
                JsonConvert.SerializeObject(
                    response, new DefaultJsonSerializerSettings()), Encoding.UTF8, "application/json");
        }
    }
}
