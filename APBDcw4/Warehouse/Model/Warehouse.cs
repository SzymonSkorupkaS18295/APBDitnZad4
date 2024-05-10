using System.ComponentModel.DataAnnotations;

namespace APBDcw4.Warehouse.Model;

public class Warehouse
{
    [Key]
    public int IdWarehouse { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}