using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreInventory.DataBase;

// Проверьте правильность пространства имен

namespace OnlineStoreInventory
{
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;

        // Конструктор, вызываемый DI
        public MainWindow(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();

            // Пример загрузки данных после инициализации окна:
            LoadCategories();
            LoadProducts();
        }

        public MainWindow() : this(
            ((App)Application.Current).ServiceProvider.GetRequiredService<ApplicationDbContext>())
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

        // Загрузка продуктов в ListBox
        private void LoadProducts()
        {
            var products = _context.Products.ToList(); // Получаем все продукты из базы данных
            ProductListBox.ItemsSource = products; // Привязываем их к ListBox
        }

        // Обработчик клика по кнопке "Add Product"
        private void OnAddProductClick(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей
            var productName = ProductNameTextBox.Text;
            var barcode = BarcodeTextBox.Text;
            var price = 0m;
            var weight = 0f;
            var minStock = 0;

            // Пробуем преобразовать введенные значения в числовые
            if (!decimal.TryParse(PriceTextBox.Text, out price) ||
                !float.TryParse(WeightTextBox.Text, out weight) ||
                !int.TryParse(MinStockTextBox.Text, out minStock))
            {
                MessageBox.Show("Please enter valid values for price, weight, and minimum stock.");
                return;
            }

            var categoryId = (int?)CategoryComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(productName) || categoryId == null || string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Создаем новый объект продукта
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

            // Добавляем новый продукт в базу данных
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            // Очищаем поля
            ProductNameTextBox.Clear();
            BarcodeTextBox.Clear();
            PriceTextBox.Clear();
            WeightTextBox.Clear();
            DimensionsTextBox.Clear();
            MinStockTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;

            // Показываем успешное сообщение
            MessageBox.Show("Product added successfully!");

            // Обновляем список продуктов
            LoadProducts();
        }

        // Обработчик клика по кнопке "Delete Product"
        private void OnDeleteProductClick(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный продукт из ListBox
            var selectedProduct = ProductListBox.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            // Запрашиваем подтверждение у пользователя
            var result = MessageBox.Show($"Are you sure you want to delete product \"{selectedProduct.Name}\"?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем выбранный продукт из базы данных
                    _context.Products.Remove(selectedProduct);
                    _context.SaveChanges();

                    MessageBox.Show("Product deleted successfully!");

                    // Обновляем список продуктов
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the product: {ex.Message}");
                }
            }
        }

        private void OnUpdateProductClick(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный товар из ListBox
            var selectedProduct = ProductListBox.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            bool updated = false;

            // Если имя изменилось, обновляем его
            if (!string.IsNullOrWhiteSpace(ProductNameTextBox.Text) &&
                ProductNameTextBox.Text != selectedProduct.Name)
            {
                selectedProduct.Name = ProductNameTextBox.Text;
                updated = true;
            }

            // Если штрихкод изменился, обновляем его
            if (!string.IsNullOrWhiteSpace(BarcodeTextBox.Text) &&
                BarcodeTextBox.Text != selectedProduct.Barcode)
            {
                selectedProduct.Barcode = BarcodeTextBox.Text;
                updated = true;
            }

            // Если цена изменена, пытаемся распарсить и обновить
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
                    MessageBox.Show("Invalid price value.");
                    return;
                }
            }

            // Если вес изменился, пытаемся распарсить и обновить
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
                    MessageBox.Show("Invalid weight value.");
                    return;
                }
            }

            // Если размеры изменились, обновляем их
            if (!string.IsNullOrWhiteSpace(DimensionsTextBox.Text) &&
                DimensionsTextBox.Text != selectedProduct.Dimensions)
            {
                selectedProduct.Dimensions = DimensionsTextBox.Text;
                updated = true;
            }

            // Если минимальный остаток изменился, пытаемся распарсить и обновить
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
                    MessageBox.Show("Invalid minimum stock value.");
                    return;
                }
            }

            // Если категория изменена, обновляем её
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
                MessageBox.Show("No changes detected.");
                return;
            }

            try
            {
                _context.Products.Update(selectedProduct);
                _context.SaveChanges();

                MessageBox.Show("Product updated successfully!");
                LoadProducts(); // Обновляем список продуктов, если требуется
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }
        
        private void OnShowGroupedProductsClick(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно и передаем в него текущий контекст БД
            var groupedWindow = new GroupedProductsWindow(_context);
            groupedWindow.Show();
        }
        
        private void OnShowLowStockAlertsClick(object sender, RoutedEventArgs e)
        {
            
            var alertsWindow = new LowStockAlertsWindow(_context);
            alertsWindow.Show();
        }
        
        private void OnOpenProductSearchClick(object sender, RoutedEventArgs e)
        {

            var searchWindow = new ProductSearchWindow(_context);
            searchWindow.Show();
        }
        
        private void OnOpenReportsClick(object sender, RoutedEventArgs e)
        {
            
            var reportsWindow = new ReportsWindow(_context);
            reportsWindow.Show();
        }

    }
}