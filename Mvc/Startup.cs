using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using Models;
using Services.Fakers;
using Services.Fakers.Models;

namespace Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            var cultures = new [] {"en-Us", "pl"};
            services.Configure<RequestLocalizationOptions>(options => {
                options.SetDefaultCulture(cultures.First());
                options.AddSupportedCultures(cultures);
                options.AddSupportedUICultures(cultures);
                //options.FallBackToParentCultures = true;
            });

            services.AddControllersWithViews()
                .AddViewLocalization(/*Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix*/)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Program)));


            services.AddDirectoryBrowser();
            services.AddSingleton<ICrudServiceAsync<User>> (x => new CrudService<User>(new UserFaker(), 10));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Download")),
                RequestPath = "/Download",
                //using Microsoft.AspNetCore.Http;
                OnPrepareResponse = ctx => {ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=60000");}
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Download")),
                RequestPath = "/Download"
            });

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
