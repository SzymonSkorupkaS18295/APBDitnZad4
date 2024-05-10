using System.Data;
using APBDcw4.Warehouse.DTO;
using APBDcw4.Warehouse.Service;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace APBDcw4.Warehouse.Controller;


[ApiController]
[Route("[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly WarehouseService _warehouseService;

    public WarehouseController(IConfiguration configuration, WarehouseService warehouseService)
    {
        _configuration = configuration;
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouseEntryAsync([FromBody] WarehouseEntryDTO entry)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _warehouseService.CreateWarehouseEntryAsync(entry);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("AddProductToWarehouse")]
    public async Task<IActionResult> AddProductToWarehouseAsync([FromBody] WarehouseEntryDTO entry)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("WarehouseDatabase")))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddProductToWarehouse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdProduct", entry.IdProduct);
                    command.Parameters.AddWithValue("@IdWarehouse", entry.IdWarehouse);
                    command.Parameters.AddWithValue("@Amount", entry.Amount);
                    command.Parameters.AddWithValue("@CreatedAt", entry.CreatedAt);

                    // Execute the procedure
                    var result = await command.ExecuteNonQueryAsync();

                    return Ok(new { message = "Poprawnie dodano produkt do magazynu." });
                }
            }
        }
        catch (SqlException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
