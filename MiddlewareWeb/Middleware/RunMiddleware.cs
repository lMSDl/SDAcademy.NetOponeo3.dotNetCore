using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MiddlewareWeb.Middleware
{
    public class RunMiddleware
    {
        public RunMiddleware(RequestDelegate _)
        {
        }

        public async Task InvokeAsync(HttpContext context) {
                System.Console.WriteLine("Begin Run");
                await context.Response.WriteAsync("Hello from Run!");
                System.Console.WriteLine("End Run");
        }
    }
}