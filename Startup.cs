using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.CheckConsentNeeded = context => true;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseMiddleware<ConsentMiddleware>();
            app.UseSession();
            app.UseRouting();

            app.Use(async (context, next) => 
            {
                await context.Response.WriteAsync($"HTTPS Request: {context.Request.IsHttps} \n");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/cookie", async context =>
                {
                    int counter1 = (context.Session.GetInt32("counter1") ?? 0) + 1;
                    int counter2 = (context.Session.GetInt32("counter2") ?? 0) + 1;
                    context.Session.SetInt32("counter1", counter1);
                    context.Session.SetInt32("counter2", counter2);
                    await context.Session.CommitAsync();
                    await context.Response.WriteAsync($"Conuter1: {counter1}, Counter2: {counter2}");
                });

                endpoints.MapFallback(async context =>
                    await context.Response.WriteAsync("Hello World!"));

            });
        }
    }
}
