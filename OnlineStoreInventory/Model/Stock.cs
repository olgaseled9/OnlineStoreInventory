using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreInventory;

public class Stock
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public string Location { get; set; }
    public bool IsInReserve { get; set; } = false;

    public Product Product { get; set; }
}