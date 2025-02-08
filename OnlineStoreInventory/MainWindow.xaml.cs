using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;

        // Конструктор, вызываемый через DI
        public MainWindow(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();

            // Загрузка данных при инициализации окна
            LoadCategories();
            LoadProducts();
        }

        // Конструктор по умолчанию для XAML (делегирует DI)
        public MainWindow() : this(App.ServiceProvider.GetRequiredService<ApplicationDbContext>())
        {
        }

        // Загрузка категорий для ComboBox
        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
        }

        // Загрузка продуктов в ListView
        private void LoadProducts()
        {
            var products = _context.Products.ToList(); // Получаем все продукты из базы данных
            ProductListView.ItemsSource = products; // Привязываем их к ListView
        }

        // Обработчик клика по кнопке "Добавить товар"
        private void OnAddProductClick(object sender, RoutedEventArgs e)
        {
            var productName = ProductNameTextBox.Text;
            var barcode = BarcodeTextBox.Text;
            var price = 0m;
            var weight = 0f;
            var minStock = 0;

            if (!decimal.TryParse(PriceTextBox.Text, out price) ||
                !float.TryParse(WeightTextBox.Text, out weight) ||
                !int.TryParse(MinStockTextBox.Text, out minStock))
            {
                MessageBox.Show("Введите корректные значения цены, веса и минимального запаса.");
                return;
            }

            var categoryId = (int?)CategoryComboBox.SelectedValue;
            if (string.IsNullOrWhiteSpace(productName) || categoryId == null || string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var newProduct = new Product
            {
                Name = productName,
                CategoryId = categoryId.Value,
                Barcode = barcode,
                Price = price,
                Weight = weight,
                Dimensions = DimensionsTextBox.Text,
                MinStock = minStock,
                Category = _context.Categories.FirstOrDefault(c => c.Id == categoryId.Value)
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            ProductNameTextBox.Clear();
            BarcodeTextBox.Clear();
            PriceTextBox.Clear();
            WeightTextBox.Clear();
            DimensionsTextBox.Clear();
            MinStockTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;

            MessageBox.Show("Товар успешно добавлен!");
            LoadProducts();
        }

        // Обработчик клика по кнопке "Удалить товар"
        private void OnDeleteProductClick(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductListView.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.");
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар \"{selectedProduct.Name}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Products.Remove(selectedProduct);
                    _context.SaveChanges();

                    MessageBox.Show("Товар успешно удален!");
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении товара: {ex.Message}");
                }
            }
        }

        // Обработчик клика по кнопке "Обновить товар"
        private void OnUpdateProductClick(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductListView.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар для обновления.");
                return;
            }

            bool updated = false;

            if (!string.IsNullOrWhiteSpace(ProductNameTextBox.Text) &&
                ProductNameTextBox.Text != selectedProduct.Name)
            {
                selectedProduct.Name = ProductNameTextBox.Text;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(BarcodeTextBox.Text) &&
                BarcodeTextBox.Text != selectedProduct.Barcode)
            {
                selectedProduct.Barcode = BarcodeTextBox.Text;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                if (decimal.TryParse(PriceTextBox.Text, out decimal newPrice))
                {
                    if (newPrice != selectedProduct.Price)
                    {
                        selectedProduct.Price = newPrice;
                        updated = true;
                    }
                }
                else
                {
                    MessageBox.Show("Недопустимое значение цены.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(WeightTextBox.Text))
            {
                if (float.TryParse(WeightTextBox.Text, out float newWeight))
                {
                    if (newWeight != selectedProduct.Weight)
                    {
                        selectedProduct.Weight = newWeight;
                        updated = true;
                    }
                }
                else
                {
                    MessageBox.Show("Неверное значение веса.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(DimensionsTextBox.Text) &&
                DimensionsTextBox.Text != selectedProduct.Dimensions)
            {
                selectedProduct.Dimensions = DimensionsTextBox.Text;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(MinStockTextBox.Text))
            {
                if (int.TryParse(MinStockTextBox.Text, out int newMinStock))
                {
                    if (newMinStock != selectedProduct.MinStock)
                    {
                        selectedProduct.MinStock = newMinStock;
                        updated = true;
                    }
                }
                else
                {
                    MessageBox.Show("Неверное значение минимального запаса.");
                    return;
                }
            }

            if (CategoryComboBox.SelectedValue != null)
            {
                int newCategoryId = (int)CategoryComboBox.SelectedValue;
                if (newCategoryId != selectedProduct.CategoryId)
                {
                    selectedProduct.CategoryId = newCategoryId;
                    selectedProduct.Category = _context.Categories.FirstOrDefault(c => c.Id == newCategoryId);
                    updated = true;
                }
            }

            if (!updated)
            {
                MessageBox.Show("Изменения не обнаружены.");
                return;
            }

            try
            {
                _context.Products.Update(selectedProduct);
                _context.SaveChanges();

                MessageBox.Show("Продукт успешно обновлен!");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления продукта: {ex.Message}");
            }
        }

        // Открытие окна с группировкой товаров
        private void OnShowGroupedProductsClick(object sender, RoutedEventArgs e)
        {
            var groupedWindow = new GroupedProductsWindow(_context);
            groupedWindow.Show();
        }

        // Открытие окна с оповещениями о низком остатке
        private void OnShowLowStockAlertsClick(object sender, RoutedEventArgs e)
        {
            var alertsWindow = new LowStockAlertsWindow(_context);
            alertsWindow.Show();
        }

        // Открытие окна расширенного поиска товаров
        private void OnOpenProductSearchClick(object sender, RoutedEventArgs e)
        {
            var searchWindow = new ProductSearchWindow(_context);
            searchWindow.Show();
        }

        // Открытие окна отчётов и аналитики
        private void OnOpenReportsClick(object sender, RoutedEventArgs e)
        {
            var reportsWindow = new ReportsWindow(_context);
            reportsWindow.Show();
        }
    }
}
