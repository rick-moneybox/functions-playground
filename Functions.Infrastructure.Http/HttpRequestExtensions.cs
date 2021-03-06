﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Functions.Infrastructure
{
    public static class HttpRequestExtensions
    {
        public static async Task<TRequest> DeserializeRequestBodyAsync<TRequest>(this HttpRequest request)
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            return JsonConvert.DeserializeObject<TRequest>(requestBody);
        }
    }
}
