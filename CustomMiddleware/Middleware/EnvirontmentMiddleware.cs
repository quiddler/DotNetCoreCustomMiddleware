using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Ui.Middleware
{
    public class EnvironmentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;

        public EnvironmentMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            // add custom header for development environment
            context.Response.Headers.Add("X-HostingEnvironment", new [] {_env.EnvironmentName});

            await _next(context);

            // do this after all other middleware has completed
            if (_env.IsDevelopment() && context.Response.ContentType == "text/html")
            {
                await context.Response.WriteAsync($"<p>From {_env.EnvironmentName}</p>");
            }
        }
    }
}
