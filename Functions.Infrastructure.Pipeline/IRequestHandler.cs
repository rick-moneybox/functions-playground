using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Pipeline
{
    public interface IRequestHandler<TFunctionParams>
    {
        Task<HttpResponseMessage> Handle(TFunctionParams @params);
    }
}
