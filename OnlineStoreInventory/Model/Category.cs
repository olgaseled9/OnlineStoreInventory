namespace OnlineStoreInventory;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public string GetCategoryInfo()
    {
        return $"Category ID: {Id}, Name: {Name}, Description: {Description}";
    }
}
