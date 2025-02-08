using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreInventory;

[Table("Report")]
public class Report
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
}
