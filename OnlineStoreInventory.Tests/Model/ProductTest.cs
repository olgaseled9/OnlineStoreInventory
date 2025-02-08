using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory.Tests.Model;

[TestClass]
[TestSubject(typeof(Product))]
public class ProductTest
{
    // Метод для создания контекста с in-memory базой данных.
    private ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        return new ApplicationDbContext(options);
    }

    // Тест: Добавление товара сохраняет его в базу данных
    [TestMethod]
    public async Task AddProduct_SavesToDatabase()
    {
        // Arrange: создаем контекст и новый объект товара
        using var context = CreateInMemoryDbContext();
        var product = new Product
        {
            Name = "Тестовый товар",
            CategoryId = 1,
            Barcode = "1234567890",
            Price = 10.0m,
            Weight = 1.0f,
            Dimensions = "10x10x10",
            MinStock = 5
        };

        // Act: добавляем товар и сохраняем изменения
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Assert: проверяем, что товар сохранён в базе данных
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Barcode == "1234567890");
        Assert.IsNotNull(savedProduct, "Товар не найден в базе данных после сохранения.");
        Assert.AreEqual("Тестовый товар", savedProduct.Name, "Название товара не совпадает.");
    }

    // Тест: Обновление товара изменяет данные в базе
    [TestMethod]
    public async Task UpdateProduct_ChangesValues()
    {
        // Arrange: создаем контекст и добавляем первоначальный товар
        using var context = CreateInMemoryDbContext();
        var product = new Product
        {
            Name = "Начальное имя",
            CategoryId = 1,
            Barcode = "ABC123",
            Price = 20.0m,
            Weight = 2.0f,
            Dimensions = "20x20x20",
            MinStock = 10
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act: изменяем свойства товара, обновляем и сохраняем изменения
        product.Name = "Обновленное имя";
        product.Price = 25.0m;
        context.Products.Update(product);
        await context.SaveChangesAsync();

        // Assert: проверяем, что изменения применены
        var updatedProduct = await context.Products.FirstOrDefaultAsync(p => p.Barcode == "ABC123");
        Assert.IsNotNull(updatedProduct, "Товар не найден после обновления.");
        Assert.AreEqual("Обновленное имя", updatedProduct.Name, "Имя товара не обновлено.");
        Assert.AreEqual(25.0m, updatedProduct.Price, "Цена товара не обновлена.");
    }
}
