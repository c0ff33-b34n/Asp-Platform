using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndPointExtensions
    {
        public static void MapWeather(this IEndpointRouteBuilder app, string path)
        {
            IResponseFormatter formatter = app.ServiceProvider.GetService<IResponseFormatter>();
            app.MapGet(path, context => Platform.WeatherEndpoint.Endpoint(context, formatter));
        }
    }
}