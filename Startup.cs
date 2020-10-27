using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseDeveloperExceptionPage();
            // app.UseMiddleware<Population>();
            // app.UseMiddleware<Capital>();
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("{first}/{second}/{third}", async context =>
                {
                    await context.Response.WriteAsync("Request Was Routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });
                
                endpoints.MapGet("capital/uk", new Capital().Invoke);
                endpoints.MapGet("population/paris", new Population().Invoke);
            });

            app.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            );
        }
    }
}
