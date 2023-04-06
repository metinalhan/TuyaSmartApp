using Microsoft.Extensions.DependencyInjection;
using TuyaApp.Application.Abstractions.DeviceModels;
using TuyaApp.Application.Abstractions.MenuLogic;
using TuyaApp.Application.Abstractions.Services;
using TuyaApp.Infrastructure.Concretes.DeviceModels;
using TuyaApp.Infrastructure.Concretes.MenuLogic;
using TuyaApp.Infrastructure.Concretes.Services;

namespace TuyaApp.Infrastructure
{
    public static class ServiceRegistration
    {      
        public static void AddInfrastructureServices(this IServiceCollection services)
        {           
            services.AddScoped<IMainViewModel, MainViewModel>();

            services.AddScoped<IMenuLogicService, MenuLogicService>();
            services.AddScoped<ITuyaCloudService, TuyaCloudService>();
            services.AddScoped<ISmartDeviceFactory, SmartDeviceFactory>();
            services.AddScoped<IDashboardViewService, DashboardViewService>();
        }
    }
}
