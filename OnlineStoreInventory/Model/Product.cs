namespace OnlineStoreInventory;

public class Product
{

    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Barcode { get; set; }
    public decimal Price { get; set; }
    public float Weight { get; set; }
    public string Dimensions { get; set; }
    public int MinStock { get; set; }
    public Category Category { get; set; }
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

}
