using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            // app.UseMiddleware<Population>();
            // app.UseMiddleware<Capital>();
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("{first:alpha:length(3)}/{second:bool}", async context =>
                {
                    await context.Response.WriteAsync("Request Was Routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });

                endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);
                
                endpoints.MapGet("size/{city?}", Population.Endpoint)
                    .WithMetadata(new RouteNameMetadata("population"));
                
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync("Routed to fallback endpoint");
                });
            });

            app.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            );
        }
    }
}
