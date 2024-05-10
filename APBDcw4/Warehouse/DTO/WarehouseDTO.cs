namespace APBDcw4.Warehouse.DTO;

using System;
using System.ComponentModel.DataAnnotations;

public class WarehouseEntryDTO
{
    [Required]
    public int IdProduct { get; set; }

    [Required]
    public int IdWarehouse { get; set; }

    [Required]
    [Range(0.0, Double.MaxValue, ErrorMessage = "Ilosc musi wynosic wiecej niz  0!")]
    public int Amount { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}
