using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            services.AddSingleton(typeof(ICollection<>), typeof(List<>));
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.UseEndpoints(endpoints => {

                endpoints.MapGet("/string", async context =>
                {
                    ICollection<string> collection
                        = context.RequestServices.GetService<ICollection<string>>();
                    collection.Add($"Request: { DateTime.Now.ToLongTimeString() }");
                    foreach (string str in collection)
                    {
                        await context.Response.WriteAsync($"String: {str}\n");
                    }
                });
                
                endpoints.MapGet("/int", async context =>
                {
                    ICollection<int> collection
                        = context.RequestServices.GetService<ICollection<int>>();
                    collection.Add(collection.Count() + 1);
                    foreach (int val in collection)
                    {
                        await context.Response.WriteAsync($"Int: {val}\n");
                    }
                });
            });
        }
    }
}
