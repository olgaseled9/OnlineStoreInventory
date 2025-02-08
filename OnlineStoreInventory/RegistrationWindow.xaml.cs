using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class RegistrationWindow : Window
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Конструктор получает зависимости через DI
        public RegistrationWindow(UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            InitializeComponent();
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private async void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string address = AddressTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            // Проверяем, существует ли выбранная роль, и если нет, создаем её
            if (!await _roleManager.RoleExistsAsync(role))
            {
                IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    MessageBox.Show("Не удалось создать роль.");
                    return;
                }
            }

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FullName = fullName,   // Устанавливаем полное имя
                Address = address      // Устанавливаем адрес
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                // Назначаем выбранную роль пользователю
                IdentityResult addRoleResult = await _userManager.AddToRoleAsync(newUser, role);
                if (addRoleResult.Succeeded)
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован и назначена роль.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь зарегистрирован, но не удалось назначить роль.");
                }
            }
            else
            {
                string errors = string.Join("\n", result.Errors.Select(e => e.Description));
                MessageBox.Show($"Ошибка регистрации:\n{errors}");
            }
        }
    }
}
