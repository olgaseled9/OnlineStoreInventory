using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreInventory;

public class Supply

{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Supplier { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal TotalCost { get; set; }

    public Product Product { get; set; }
}

