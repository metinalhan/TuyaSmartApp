using Microsoft.Extensions.DependencyInjection;
using TuyaApp.Application.Abstractions.Repositories;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Persistence.Context;
using TuyaApp.Persistence.Repositories;
using TuyaApp.Persistence.Services;

namespace TuyaApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {     
            services.AddDbContext<TuyaAppDbContext>();          
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ITuyaAccountRepository, TuyaAccountRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ITuyaAccountService, TuyaAccountService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
