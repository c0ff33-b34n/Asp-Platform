using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Platform
{
    public class Startup
    {

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        
        private IConfiguration Configuration;
        public void ConfigureServices(IServiceCollection services) {
            services.AddScoped<IResponseFormatter>(serviceProvider =>
            {
                string typeName = Configuration["services:IResponseFormatter"];
                return (IResponseFormatter)ActivatorUtilities
                    .CreateInstance(serviceProvider, typeName == null
                    ? typeof(GuidService) : Type.GetType(typeName, true));
            });
            services.AddScoped<ITimeStamper, DefaultTimeStamper>();
        }

        public void Configure(IApplicationBuilder app, IResponseFormatter formatter)
        {
            
            app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.UseMiddleware<WeatherMiddleware>();

            app.Use(async (context, next) => {
                if (context.Request.Path == "/middleware/function") {
                    IResponseFormatter formatter
                        = context.RequestServices.GetService<IResponseFormatter>();
                    await formatter.Format(context,
                        "Middleware Function: It is snowing in Chicago");
                } else {
                    await next();
                }
            });

            app.UseEndpoints(endpoints => {

                endpoints.MapEndpoint<WeatherEndpoint>("/endpoint/class");

                endpoints.MapGet("/endpoint/function", async context => {
                    IResponseFormatter formatter
                        = context.RequestServices.GetService<IResponseFormatter>();
                    await formatter.Format(context,
                        "Endpoint Function: It is sunny in LA");
                });
            });
        }
    }
}
