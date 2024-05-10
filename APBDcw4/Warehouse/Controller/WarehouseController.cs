using APBDcw4.Warehouse.DTO;
using APBDcw4.Warehouse.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBDcw4.Warehouse.Controller;


[ApiController]
[Route("[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseService _warehouseService;

    public WarehouseController(WarehouseService warehouseService)
    {
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
}