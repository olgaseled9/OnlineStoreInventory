using Microsoft.EntityFrameworkCore;
using OnlineStoreInventory.DataBase;

namespace OnlineStoreInventory.Services;

public class StockService : IStockService
{
    private readonly ApplicationDbContext _context;

    public StockService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stock>> GetAllStockAsync() =>
        await _context.Stocks.Include(s => s.Product).ToListAsync();

    public async Task<Stock> GetStockByIdAsync(int id) =>
        await _context.Stocks.Include(s => s.Product).FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddStockAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStockAsync(Stock stock)
    {
        _context.Stocks.Update(stock);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStockAsync(int id)
    {
        var stock = await GetStockByIdAsync(id);
        if (stock != null)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }
    }

    // 📌 Резервирование товара
    public async Task<bool> ReserveStockAsync(int stockId, int quantity)
    {
        var stock = await GetStockByIdAsync(stockId);
        if (stock == null || stock.Quantity < quantity)
            return false; // Недостаточно товара на складе

        stock.Quantity -= quantity;
        stock.IsInReserve = true;
        await _context.SaveChangesAsync();
        return true;
    }

    // 📌 Освобождение зарезервированного товара
    public async Task<bool> ReleaseStockAsync(int stockId, int quantity)
    {
        var stock = await GetStockByIdAsync(stockId);
        if (stock == null || !stock.IsInReserve)
            return false; // Товар не в резерве

        stock.Quantity += quantity;
        stock.IsInReserve = false;
        await _context.SaveChangesAsync();
        return true;
    }

    // 📌 Получение всех зарезервированных товаров
    public async Task<IEnumerable<Stock>> GetReservedStockAsync() =>
        await _context.Stocks.Where(s => s.IsInReserve).Include(s => s.Product).ToListAsync();

    // 📌 Получение всех доступных товаров (не в резерве)
    public async Task<IEnumerable<Stock>> GetAvailableStockAsync() =>
        await _context.Stocks.Where(s => !s.IsInReserve).Include(s => s.Product).ToListAsync();
}