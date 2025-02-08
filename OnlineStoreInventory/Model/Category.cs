using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreInventory;

[Table("Category")]
public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public string GetCategoryInfo()
    {
        return $"Category ID: {Id}, Name: {Name}, Description: {Description}";
    }
}
