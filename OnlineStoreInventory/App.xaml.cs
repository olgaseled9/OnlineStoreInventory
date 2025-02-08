using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OnlineStoreInventory.DataBase; 
using System;
using System.Windows;

namespace OnlineStoreInventory
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Настройка подключения к БД с Windows-аутентификацией
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=OnlineStoreInventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"));


                    // Регистрируем главное окно
                    services.AddSingleton<MainWindow>();
                })
                .Build();

            ServiceProvider = _host.Services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}