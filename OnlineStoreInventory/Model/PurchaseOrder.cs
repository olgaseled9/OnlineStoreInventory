namespace OnlineStoreInventory;

public class PurchaseOrder
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
    public Product Product { get; set; }
}
