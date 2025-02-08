using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineStoreInventory;

[Table("Product")]
public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Barcode { get; set; }
    public decimal Price { get; set; }
    
    // Измените тип с float на double:
    public double Weight { get; set; }
    
    public string Dimensions { get; set; }
    public int MinStock { get; set; }
    
    public Category Category { get; set; }
    
    public ICollection<Stock> Stock { get; set; } = new List<Stock>();
    public ICollection<Supply> Supply { get; set; } = new List<Supply>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}