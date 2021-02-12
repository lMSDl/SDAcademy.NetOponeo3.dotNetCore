using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoutingWeb.Middleware;

namespace RoutingWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LimitRequestsMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();    

            app.Use(async (context, next) => {
                    System.Console.WriteLine("Begin Use");
                    var endPoint = context.GetEndpoint();
                    System.Console.WriteLine($"{endPoint?.DisplayName ?? "none"}");
                    if(endPoint?.DisplayName.Contains("Hello") ?? false) {
                        context.Items.Add("middlewareItem", new Random().Next(10));
                    }
                    await next();
                    System.Console.WriteLine("End Use");
            });

            
            app.Map("/map", appBuilder => appBuilder.Run(async (context) => {
                    System.Console.WriteLine("Begin MapRun");
                    await context.Response.WriteAsync("Hello from Map!");
                    System.Console.WriteLine("End MapRun");
            }));

            app.UseMiddleware<LimitRequestsMiddleware>();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/oddNumber", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync(GenerateNumber(x => (x % 2) != 0).ToString());
                })
                .WithMetadata(new LimitRequestsMiddleware.LimitRequestsMiddlewareMetadata {Limit = 1});
                
                endpoints.MapGet("/evenNumber", async context =>
                {
                    await Task.Delay(5000);
                    await context.Response.WriteAsync(GenerateNumber(x => (x % 2) == 0).ToString());
                })
                .WithMetadata(new LimitRequestsMiddleware.LimitRequestsMiddlewareMetadata {Limit = 2});

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(PrepareHello());
                });

                endpoints.MapGet("/Hello", async context =>
                {
                    await context.Response.WriteAsync(PrepareHello((int)context.Items["middlewareItem"]));
                });
                endpoints.MapGet("/exception", context => throw new Exception("Something wrong!"));

            });

            app.Run(async (context) => {
                    System.Console.WriteLine("Begin Run");
                    await context.Response.WriteAsync("Hello from Run!");
                    System.Console.WriteLine("End Run");
            });
        }
        
        private static string PrepareHello(int repeat = 1) {
            var stringBuilder = new StringBuilder();
            for(var i = 0; i < repeat; i ++)
                stringBuilder.AppendLine("Hello World!");
            return stringBuilder.ToString();
        }

        private static int GenerateNumber(Func<int, bool> predicate) {
            var random = new Random();
            int result;
            do {
                result = random.Next();
            }
            while(!predicate(result));

            return result;
        }
    }
}
