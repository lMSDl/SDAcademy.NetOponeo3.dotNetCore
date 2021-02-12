using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MiddlewareWeb.Middleware
{
    public class Use1Middleware
    {
        private RequestDelegate _next;

        public Use1Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
                System.Console.WriteLine("Begin Use1");
                await _next(context);
                System.Console.WriteLine("End Use1");
        }
    }
}