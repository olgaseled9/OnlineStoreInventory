using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStoreInventory;
using OnlineStoreInventory.DataBase;
using OnlineStoreInventory.Services;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Подключение базы данных
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("Server=localhost;Database=OnlineStoreInventoryDB;Trusted_Connection=True;"));

                // Регистрация сервисов
                services.AddScoped<IStockService, StockService>();

                // Регистрация главного окна как Scoped
                services.AddScoped<MainWindow>(); 
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Получение окна из DI контейнера
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}