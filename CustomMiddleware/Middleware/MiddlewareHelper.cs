using Microsoft.AspNetCore.Builder;

namespace Ui.Middleware
{
    public static class MiddlewareHelper
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<EnvironmentMiddleware>();
        }
    }
}
