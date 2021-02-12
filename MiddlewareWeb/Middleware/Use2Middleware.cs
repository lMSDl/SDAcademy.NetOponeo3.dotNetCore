using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MiddlewareWeb.Middleware
{
    public class Use2Middleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
                System.Console.WriteLine("Begin Use2");
                await next(context);
                System.Console.WriteLine("End Use2");
        }
    }

    public static class IApplicationBuilderExtensions {
        public static IApplicationBuilder UseUse2Middleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<Use2Middleware>();
        }
    }
}