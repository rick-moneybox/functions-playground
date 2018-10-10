using Functions.Infrastructure.Responses;
using System.Net;

namespace Functions.Infrastructure
{
    public class ErrorJsonResponse : JsonResponse
    {
        public ErrorJsonResponse(HttpStatusCode statusCode, string message)
            : base(statusCode, new ErrorState(statusCode, message))
        {
        }

        class ErrorState
        {
            public string Error { get; }

            public string Message { get; }

            public ErrorState(HttpStatusCode statusCode, string message)
                : base()
            {
                this.Error = statusCode.ToString();
                this.Message = message;
            }
        }
    }
}
