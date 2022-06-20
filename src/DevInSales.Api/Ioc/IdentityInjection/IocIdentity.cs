using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Api.Ioc.IdentityInjection
{
    public static class IocIdentity
    {
        public static IServiceCollection AddIocIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
                options.EnableSensitiveDataLogging(true);
            });
            services.AddDefaultIdentity<User>()
                    .AddEntityFrameworkStores<DataContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
            });
            return services;
        }    
            
    }
}