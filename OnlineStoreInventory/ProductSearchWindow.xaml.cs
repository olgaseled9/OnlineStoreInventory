using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using OnlineStoreInventory.DataBase; // Проверьте корректное пространство имен

namespace OnlineStoreInventory
{
    public partial class ProductSearchWindow : Window
    {
        private readonly ApplicationDbContext _context;

        public ProductSearchWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
            // При открытии можно сразу выполнить поиск без фильтров, чтобы показать все товары:
            OnSearchClick(null, null);
        }

        // Метод для загрузки категорий в ComboBox
        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            CategoryComboBox.ItemsSource = categories;
        }

        // Обработчик нажатия кнопки "Search"
        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            // Начинаем с всех продуктов, включая навигационное свойство Category
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Фильтр по ключевым словам (по названию продукта)
            string searchText = SearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText));
            }

            // Фильтр по диапазону цены
            if (decimal.TryParse(MinPriceTextBox.Text, out decimal minPrice))
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if (decimal.TryParse(MaxPriceTextBox.Text, out decimal maxPrice))
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            // Фильтр по диапазону веса (учтите, что тип веса может быть float или double)
            if (float.TryParse(MinWeightTextBox.Text, out float minWeight))
            {
                query = query.Where(p => p.Weight >= minWeight);
            }
            if (float.TryParse(MaxWeightTextBox.Text, out float maxWeight))
            {
                query = query.Where(p => p.Weight <= maxWeight);
            }

            // Фильтр по категории, если выбрана
            if (CategoryComboBox.SelectedValue != null)
            {
                int selectedCategoryId = (int)CategoryComboBox.SelectedValue;
                query = query.Where(p => p.CategoryId == selectedCategoryId);
            }

            // Выполнение запроса и получение результатов
            var results = query.ToList();

            // Для обновления представления можно использовать CollectionViewSource, если нужна динамическая фильтрация,
            // но здесь достаточно напрямую установить ItemsSource:
            ResultsDataGrid.ItemsSource = results;
        }
    }
}
