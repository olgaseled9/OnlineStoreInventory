using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class LowStockAlertsWindow : Window
    {
        private readonly ApplicationDbContext _context;

        public LowStockAlertsWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadLowStockProducts();
        }

        private void LoadLowStockProducts()
        {
            // Если у товара может быть несколько записей в Stock, группируем их по ProductId:
            var lowStockProducts = (from product in _context.Products
                join stock in _context.Stocks on product.Id equals stock.ProductId into productStocks
                let totalQuantity = productStocks.Sum(s => s.Quantity)
                where totalQuantity < product.MinStock
                orderby (product.MinStock - totalQuantity) descending
                select new
                {
                    product.Id,
                    product.Name,
                    product.MinStock,
                    TotalQuantity = totalQuantity,
                    Deficit = product.MinStock - totalQuantity
                }).ToList();

            // Если требуется группировать или сортировать данные, можно создать CollectionView:
            var view = CollectionViewSource.GetDefaultView(lowStockProducts);
            // Например, сортировка по величине дефицита уже задана в запросе
            LowStockDataGrid.ItemsSource = view;
        }
    }
}