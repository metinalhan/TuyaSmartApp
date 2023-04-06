using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Forms;
using TuyaApp.Application;
using TuyaApp.Application.Abstractions.DashboardView;
using TuyaApp.Infrastructure;
using TuyaApp.Persistence;
using TuyaApp.Persistence.Context;
using WpfApp.View;
using WpfApp.ViewModel;
using Application = System.Windows.Application;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
      
        public App()
        {            
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {                    
                    ConfigureServices(services);

                }).Build();


            using (var scope = AppHost.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<TuyaAppDbContext>();

                // Here is the migration executed             
                dbContext.Database.Migrate();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();           
            services.AddSingleton<NotifyIcon>();

            services.AddTransient<MenuView>();
            services.AddTransient<AccountView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<DeviceView>();
            services.AddTransient<ProfileView>();


            services.AddSingleton<Func<MenuView>>(serviceProvider => serviceProvider.GetService<MenuView>);
            services.AddSingleton<Func<AccountView>>(serviceProvider => serviceProvider.GetService<AccountView>);
            services.AddSingleton<Func<DashboardView>>(serviceProvider => serviceProvider.GetService<DashboardView>);
            services.AddSingleton<Func<DeviceView>>(serviceProvider => serviceProvider.GetService<DeviceView>);
            services.AddSingleton<Func<ProfileView>>(serviceProvider => serviceProvider.GetService<ProfileView>);


            services.AddPersistanceServices();
            services.AddInfrastructureServices();
            services.AddApplicationServices();

           services.AddScoped<IDashboardViewFactory,DashboardViewFactory>();
          

        }

        protected override async void OnStartup(StartupEventArgs e)
        { 

            await AppHost.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }      


        protected override async void OnExit(ExitEventArgs e)
        {
            using (AppHost)
            {
                await AppHost.StopAsync();
            }
            base.OnExit(e);
        }
    }
}
