using Microsoft.Extensions.DependencyInjection;
using Models;
using Services.Fakers;
using Services.Fakers.Models;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.MsSqlService;

namespace Mvc.Extensions
{
    public static class ServiceCollectionExtensions
    {   
        public static IServiceCollection AddMsSqlServices(this IServiceCollection services, string connectionString) {
                services.AddDbContext<DbContext, Context>(options => options.UseSqlServer(connectionString)/*.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)*/);
                services.AddScoped<IUsersServiceAsync, Services.MsSqlService.Services.UsersService>();
                return services;
        }

        public static IServiceCollection AddFakerServices(this IServiceCollection services) {
            services.AddSingleton<IUsersServiceAsync> (x => new UsersService(new UserFaker(), 10));
            services.AddSingleton<ICrudServiceAsync<Tire>> (x => new CrudService<Tire>(new TireFaker(), 10));
            return services;
        }
    }
}