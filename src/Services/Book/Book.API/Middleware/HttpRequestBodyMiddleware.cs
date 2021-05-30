using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Book.API.Middleware
{
    public class HttpRequestBodyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestBodyMiddleware> _logger;

        public HttpRequestBodyMiddleware(RequestDelegate next, ILogger<HttpRequestBodyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();
            _logger.LogInformation("Request {HttpRequestMethod} : {Path}\n{body}", 
                context.Request.Method,
                context.Request.Path.Value,
                body);
            context.Request.Body.Position = 0L;
            

            await _next(context);

        }
    }
}