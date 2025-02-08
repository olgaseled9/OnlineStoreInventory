using System.Windows;
using OnlineStoreInventory.DataBase; // Проверьте правильность пространства имен

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
    }
}
