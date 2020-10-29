using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Platform
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services) {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            
            app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.Use(async (context, next) => 
            {
                string defaultDebug = config["Logging:LogLevel:Default"];
                await context.Response.WriteAsync($"The config setting is: {defaultDebug}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
