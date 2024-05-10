using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDcw4.Warehouse.Model;

public class Order
{
    [Key]
    public int IdOrder { get; set; }
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }

    [ForeignKey("IdProduct")]
    public Product Product { get; set; }
}