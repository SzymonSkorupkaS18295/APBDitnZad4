using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDcw4.Warehouse.Model;

public class Product_Warehouse
{
    [Key]
    public int IdProductWarehouse { get; set; }
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("IdWarehouse")]
    public Warehouse Warehouse { get; set; }
    [ForeignKey("IdProduct")]
    public Product Product { get; set; }
    [ForeignKey("IdOrder")]
    public Order Order { get; set; }
}