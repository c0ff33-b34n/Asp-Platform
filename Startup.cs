using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<IResponseFormatter, GuidService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IResponseFormatter formatter) {
            
            app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.UseMiddleware<WeatherMiddleware>();

            app.Use(async (context, next) => {
                if (context.Request.Path == "/middleware/function") {
                    await formatter.Format(context,
                        "Middleware Function: It is snowing in Chicago");
                } else {
                    await next();
                }
            });

            app.UseEndpoints(endpoints => {

                endpoints.MapEndpoint<WeatherEndpoint>("/endpoint/class");

                endpoints.MapGet("/endpoint/function", async context => {
                    await formatter.Format(context,
                        "Endpoint Function: It is sunny in LA");
                });
            });
        }
    }
}
