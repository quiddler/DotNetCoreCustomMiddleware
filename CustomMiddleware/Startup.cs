using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ui.Middleware;

namespace Ui
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, 
        // visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            var logger = loggerFactory.CreateLogger("Middleware Demo");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // serve static files from the wwwwroot folder
            app.UseStaticFiles();

            app.UseCustomMiddleware();

            app.Map("/Contacts", application =>
            {
                application.Run(async context => { await SendHtml(context, "<h1>here are your contacts</h1>"); });
            });

            // configured once at startup
            // when true => route is navigated to, e.g., FirefoxRoute takes over here when the user agent is Firefox
            app.MapWhen(context => context.Request.Headers["User-Agent"].First().Contains("Firefox"), FirefoxRoute);

            app.Run(async (context) =>
            {
                await SendHtml(context, "<h1>Hello world!!!</h1>");
            });
        }

        private Task SendHtml(HttpContext context, string html)
        {
            context.Response.ContentType = "text/html";
            return context.Response.WriteAsync(html);
        }

        private void FirefoxRoute(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("./wwwroot/firefox.html");
            });
        }
    }
}
