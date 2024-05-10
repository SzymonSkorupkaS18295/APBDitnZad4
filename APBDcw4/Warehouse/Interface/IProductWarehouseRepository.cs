using APBDcw4.Warehouse.Model;

namespace APBDcw4.Warehouse.Interface;

public interface IProductWarehouseRepository
{
    Task<Product_Warehouse> AddProductWarehouseEntryAsync(Product_Warehouse entry);
    Task<bool> ExistsProductWarehouseEntryByOrderIdAsync(int orderId);
}