using Microsoft.AspNetCore.Http;
using System;

namespace Functions.Infrastructure.Pipeline.Deserialization
{
    public interface IJsonDeserializableHttpFunctionParams
    {
        HttpRequest Request { get; }

        Type BodyType { get; }
    }
}
