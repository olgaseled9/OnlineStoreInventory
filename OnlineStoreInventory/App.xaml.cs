using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStoreInventory.DataBase;
using System;
using System.Windows;

namespace OnlineStoreInventory
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Регистрируем контекст базы данных
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=OnlineStoreInventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"));

                    // Регистрируем Identity-сервисы
                    services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

                    // Регистрируем окна как Transient
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<RegistrationWindow>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Открываем окно авторизации при запуске
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
    }
}