using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreInventory;

[Table("Shipment")]
public class Shipment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string DeliveryAddress { get; set; }
    public decimal ShipmentCost { get; set; }
    public Product Product { get; set; }
}
