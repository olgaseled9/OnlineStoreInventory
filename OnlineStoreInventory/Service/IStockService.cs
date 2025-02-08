namespace OnlineStoreInventory.Services;

public interface IStockService
{
    Task<IEnumerable<Stock>> GetAllStockAsync();
    Task<Stock> GetStockByIdAsync(int id);
    Task AddStockAsync(Stock stock);
    Task UpdateStockAsync(Stock stock);
    Task DeleteStockAsync(int id);

    // Новые методы управления складом
    Task<bool> ReserveStockAsync(int stockId, int quantity);
    Task<bool> ReleaseStockAsync(int stockId, int quantity);
    Task<IEnumerable<Stock>> GetReservedStockAsync();
    Task<IEnumerable<Stock>> GetAvailableStockAsync();
}