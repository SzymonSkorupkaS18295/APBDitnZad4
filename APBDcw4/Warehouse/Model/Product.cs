namespace APBDcw4.Warehouse.Model;
using System.ComponentModel.DataAnnotations;


public class Product
{
    [Key]
    public int IdProduct { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}