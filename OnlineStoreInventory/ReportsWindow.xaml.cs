using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory
{
    public partial class ReportsWindow : Window
    {
        private readonly ApplicationDbContext _context;

        public ReportsWindow(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        // Обработчик нажатия кнопки "Generate Report"
        private void OnGenerateReportClick(object sender, RoutedEventArgs e)
        {
            string reportType = ((ComboBoxItem)ReportTypeComboBox.SelectedItem)?.Content.ToString();
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(reportType))
            {
                MessageBox.Show("Пожалуйста, выберите тип отчета.");
                return;
            }

            if (reportType == "Отчет об инвентаризации")
                GenerateInventoryReport();
            else if (reportType == "Отчет о поставках")
                GenerateSupplyReport(fromDate, toDate);
            else if (reportType == "Отчет об отправке")
                GenerateShipmentReport(fromDate, toDate);
        }

        // Отчёт по запасам (инвентарный отчёт)
        private void GenerateInventoryReport()
        {
            // Объединяем данные о продуктах с данными о складе.
            var query = from product in _context.Products.Include(p => p.Category)
                join stock in _context.Stocks on product.Id equals stock.ProductId into stockGroup
                let totalStock = stockGroup.Sum(s => (int?)s.Quantity) ?? 0
                select new
                {
                    product.Id,
                    product.Name,
                    Category = product.Category.Name,
                    product.MinStock,
                    CurrentStock = totalStock,
                    Deficit = product.MinStock - totalStock
                };

            var data = query.ToList();
            ReportDataGrid.ItemsSource = data;

            // Пример диаграммы: распределение запасов по категориям
            var chartData = data.GroupBy(d => d.Category)
                .Select(g => new { Category = g.Key, TotalStock = g.Sum(x => x.CurrentStock) })
                .ToList();

            ReportChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total Stock",
                    Values = new ChartValues<int>(chartData.Select(x => x.TotalStock))
                }
            };
            ReportChart.AxisX.Clear();
            ReportChart.AxisX.Add(new Axis
            {
                Title = "Category",
                Labels = chartData.Select(x => x.Category).ToArray()
            });
            ReportChart.AxisY.Clear();
            ReportChart.AxisY.Add(new Axis
            {
                Title = "Stock",
                LabelFormatter = value => value.ToString()
            });
        }

        // Отчёт по поставкам
        private void GenerateSupplyReport(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Supplies.Include(s => s.Product).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(s => s.Date >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(s => s.Date <= toDate.Value);

            var data = query.Select(s => new
            {
                s.Id,
                s.Date,
                s.Supplier,
                ProductName = s.Product.Name,
                s.Quantity,
                s.TotalCost
            }).ToList();

            ReportDataGrid.ItemsSource = data;

            // Диаграмма: общее количество поставок по поставщикам
            var chartData = data.GroupBy(d => d.Supplier)
                .Select(g => new { Supplier = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
                .ToList();

            ReportChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Supplies",
                    Values = new ChartValues<int>(chartData.Select(x => x.TotalQuantity))
                }
            };

            ReportChart.AxisX.Clear();
            ReportChart.AxisX.Add(new Axis
            {
                Title = "Supplier",
                Labels = chartData.Select(x => x.Supplier).ToArray()
            });
            ReportChart.AxisY.Clear();
            ReportChart.AxisY.Add(new Axis
            {
                Title = "Quantity",
                LabelFormatter = value => value.ToString()
            });
        }

        // Отчёт по отгрузкам
        private void GenerateShipmentReport(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Shipments.Include(s => s.Product).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(s => s.Date >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(s => s.Date <= toDate.Value);

            var data = query.Select(s => new
            {
                s.Id,
                s.Date,
                ProductName = s.Product.Name,
                s.Quantity,
                s.DeliveryAddress,
                s.ShipmentCost
            }).ToList();

            ReportDataGrid.ItemsSource = data;

            // Диаграмма: общее количество отгрузок по продуктам
            var chartData = data.GroupBy(d => d.ProductName)
                .Select(g => new { Product = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
                .ToList();

            ReportChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Shipments",
                    Values = new ChartValues<int>(chartData.Select(x => x.TotalQuantity))
                }
            };

            ReportChart.AxisX.Clear();
            ReportChart.AxisX.Add(new Axis
            {
                Title = "Product",
                Labels = chartData.Select(x => x.Product).ToArray()
            });
            ReportChart.AxisY.Clear();
            ReportChart.AxisY.Add(new Axis
            {
                Title = "Quantity",
                LabelFormatter = value => value.ToString()
            });
        }

        // Экспорт данных в CSV
        private void OnExportToCSVClick(object sender, RoutedEventArgs e)
        {
            var data = ReportDataGrid.ItemsSource;
            if (data == null)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            // Преобразуем данные в CSV-строку.
            var lines = new List<string>();

            // Получаем заголовки столбцов
            if (ReportDataGrid.Columns.Count > 0)
            {
                var header = string.Join(",", ReportDataGrid.Columns.Select(c => c.Header.ToString()));
                lines.Add(header);
            }

            foreach (var item in data)
            {
                var properties = item.GetType().GetProperties();
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? "");
                lines.Add(string.Join(",", values));
            }

            string filePath = @"D:\MyExports\ReportExport.csv";
            File.WriteAllLines(filePath, lines);
            MessageBox.Show($"Data exported to {filePath}");
        }

        // Экспорт данных в JSON
        private void OnExportToJSONClick(object sender, RoutedEventArgs e)
        {
            var data = ReportDataGrid.ItemsSource;
            if (data == null)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            string filePath = @"D:\MyExports\ReportExport.json";
            File.WriteAllText(filePath, json);
            MessageBox.Show($"Data exported to {filePath}");
        }
    }
}