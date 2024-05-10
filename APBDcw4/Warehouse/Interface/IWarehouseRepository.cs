namespace APBDcw4.Warehouse.Interface;

public interface IWarehouseRepository
{
    Task<Model.Warehouse> GetWarehouseAsync(int id);
}