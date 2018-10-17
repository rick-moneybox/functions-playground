using Microsoft.AspNetCore.Http;
using System;

namespace Functions.Infrastructure.Pipeline.FluentValidation
{
    public interface IValidatableFunctionParams
    {
        HttpRequest Request { get; }

        Type BodyType { get; }
    }
}
