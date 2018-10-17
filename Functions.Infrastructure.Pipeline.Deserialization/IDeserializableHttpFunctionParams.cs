using Microsoft.AspNetCore.Http;
using System;

namespace Functions.Infrastructure.Pipeline.Deserialization
{
    public interface IDeserializableHttpFunctionParams
    {
        HttpRequest Request { get; }

        Type BodyType { get; }
    }
}
