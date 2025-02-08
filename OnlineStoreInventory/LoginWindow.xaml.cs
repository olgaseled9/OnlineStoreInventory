using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class LoginWindow : Window
    {
        private readonly UserManager<ApplicationUser> _userManager;
        // Используем только UserManager для проверки логина, так как SignInManager требует HttpContext (веб-сценарий)

        // Конструктор получает зависимости через DI
        public LoginWindow(UserManager<ApplicationUser> userManager)
        {
            InitializeComponent();
            _userManager = userManager;
        }

        // Обработчик кнопки "Войти"
        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль.");
                return;
            }

            // Находим пользователя по имени
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return;
            }

            // Проверяем пароль
            bool validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (validPassword)
            {
                MessageBox.Show("Авторизация успешна!");

                // Переходим на главное окно
                var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                // Закрываем окно авторизации
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        // Обработчик кнопки "Регистрация"
        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            // Открываем окно регистрации
            var registrationWindow = App.ServiceProvider.GetRequiredService<RegistrationWindow>();
            registrationWindow.Show();

            // Опционально: можно закрыть окно логина, если регистрация подразумевает переход в другое окно
            this.Close();
        }
    }
}
