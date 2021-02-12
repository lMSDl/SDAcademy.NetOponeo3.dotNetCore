using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddlewareWeb.Middleware;

namespace MiddlewareWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Middleware.Use2Middleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseMiddleware<Middleware.Use1Middleware>()
                .UseUse2Middleware()
                .Map("/map1", appMap1 => ConfigureMap1(appMap1));
            
            app.MapWhen(context => context.Request.Query.TryGetValue("key", out var value) ? value == "map2" : false,
            appMapWhen => {
                appMapWhen.Use(async (context, next) => {
                    System.Console.WriteLine("Begin MapWhen Use");
                    await next();
                    System.Console.WriteLine("End MapWhen Use");
                });
                appMapWhen.Run(async context => {
                    System.Console.WriteLine("Begin MapWhen Run");
                    await context.Response.WriteAsync("Hello from MapWhen Run!");
                    System.Console.WriteLine("End MapWhen Run");
                });
            });

            app.Use(async (context, next) => {
                System.Console.WriteLine("Begin Use3");
                await next();
                System.Console.WriteLine("End Use3");
            });
            
            app.UseMiddleware<RunMiddleware>();

            app.Use(async (context, next) => {
                System.Console.WriteLine("Begin Use after Run");
                await next();
                System.Console.WriteLine("End Use after Run");
            });
        }
        private static void ConfigureMap1(IApplicationBuilder app) {

            app.Use(async (context, next) => {
                System.Console.WriteLine("Begin Map1 Use1");
                await next();
                System.Console.WriteLine("End Map1 Use1");
            });

            app.Run(async context => {
                System.Console.WriteLine("Begin Map1 Run");
                await context.Response.WriteAsync("Hello from Map1 Run!");
                System.Console.WriteLine("End Map1 Run");
            });
        }
    }
}
