using Microsoft.AspNetCore.Http;
using Platform.Services;
using System.Threading.Tasks;

namespace Platform {
    public class WeatherMiddleware {
        private RequestDelegate next;
        
        public WeatherMiddleware(RequestDelegate nextDelegate) 
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context, IResponseFormatter formatter) {
            if (context.Request.Path == "/middleware/class") {
                await formatter.Format(context,
                    "Middleware Class: It is raining in London");
            } else {
                await next(context);
            }
        }
    }
}