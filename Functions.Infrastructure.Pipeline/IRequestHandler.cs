using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public interface IRequestHandler
    {
        Task<IActionResult> Handle(HttpRequest request, ILogger logger);
    }
}
