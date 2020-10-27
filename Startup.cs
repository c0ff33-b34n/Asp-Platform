using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<RouteOptions>(opts =>
            {
                opts.ConstraintMap.Add("countryName",typeof(CountryRouteConstraint));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.UseMiddleware<WeatherMiddleware>();

            app.Use(async (context, next) => {
                if (context.Request.Path == "/middleware/function") {
                    await TypeBroker.Formatter.Format(context,
                        "Middleware Function: It is snowing in Chicago");
                } else {
                    await next();
                }
            });

            app.UseEndpoints(endpoints => {

                endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);

                endpoints.MapGet("/endpoint/function", async context => {
                    await TypeBroker.Formatter.Format(context,
                        "Endpoint Function: It is sunny in LA");
                });
            });
        }
    }
}
