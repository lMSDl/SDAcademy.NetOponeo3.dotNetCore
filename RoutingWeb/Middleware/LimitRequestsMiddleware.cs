using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RoutingWeb.Middleware
{
    public class LimitRequestsMiddleware : MarshalByRefObject, IMiddleware
    {
        public int _counter = 0;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var metadata = context.GetEndpoint()?.Metadata.GetMetadata<LimitRequestsMiddlewareMetadata>();
            if(metadata == null) {
                await next(context);
                return;
            }

            if(_counter  >= metadata.Limit) {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }
                _counter++;
                try {
                    System.Console.WriteLine($"Counter: {_counter}");
                    await next(context);
                }
                catch {
                    throw;
                }
                finally {
                    _counter--;
                }
        }

        public class LimitRequestsMiddlewareMetadata {
            public int Limit {get; set;}
        }
    }
}