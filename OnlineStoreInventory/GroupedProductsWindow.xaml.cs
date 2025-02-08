using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class GroupedProductsWindow : Window
    {
        private readonly ApplicationDbContext _context;

        // Конструктор принимает контекст БД через DI (или передается из основного окна)
        public GroupedProductsWindow(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();
            LoadGroupedProducts();
        }

        private void LoadGroupedProducts()
        {
            // Загружаем продукты с информацией о категории
            var products = _context.Products.Include(p => p.Category).ToList();

            // Получаем представление для группировки
            var view = CollectionViewSource.GetDefaultView(products);
            view.GroupDescriptions.Clear();
            // Группировка по имени категории (свойство Category.Name)
            view.GroupDescriptions.Add(new PropertyGroupDescription("Category.Name"));

            GroupedProductsListView.ItemsSource = view;
        }
    }
}