using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System.Threading.Tasks;

namespace Platform {
    public class WeatherEndpoint
    {
        private IResponseFormatter formatter;
        
        public WeatherEndpoint(IResponseFormatter responseFormatter)
        {
            formatter = responseFormatter;
        }

        public async Task Endpoint(HttpContext context)
        {
            await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        }
    }
}