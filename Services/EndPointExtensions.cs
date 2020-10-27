using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndPointExtensions
    {
        public static void MapEndpoint<T> (this IEndpointRouteBuilder app,
            string path, string methodName = "Endpoint")
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new System.Exception("Method cannot be used");
            }
            T endpointInstance =ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            app.MapGet(path,(RequestDelegate)methodInfo
                .CreateDelegate(typeof(RequestDelegate), endpointInstance));}
    }
}