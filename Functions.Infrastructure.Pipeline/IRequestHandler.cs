using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public interface IRequestHandler
    {
        Task<HttpResponseMessage> Handle(HttpRequest request, ILogger logger);
    }
}
