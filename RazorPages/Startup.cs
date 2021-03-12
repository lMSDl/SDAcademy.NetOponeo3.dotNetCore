using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Interfaces;
using Services.MsSqlService;

namespace RazorPages
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
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/Users");
                options.Conventions.AuthorizeFolder("/Login").AllowAnonymousToPage("/Login/Index");
                //options.Conventions.AuthorizePage("/Users/Add");
            });

            services.AddDbContext<DbContext, Context>(options => options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnectionString")));
                services.AddScoped<IUsersServiceAsync, Services.MsSqlService.Services.UsersService>();

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(cookieOptions => {
                cookieOptions.LoginPath = "/Login";
                //cookieOptions.LogoutPath = "/Login/Logout";
                cookieOptions.AccessDeniedPath = "/";
                cookieOptions.ExpireTimeSpan = TimeSpan.FromHours(1);
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
