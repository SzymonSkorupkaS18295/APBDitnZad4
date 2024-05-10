using APBDcw4.Warehouse.Model;

namespace APBDcw4.Warehouse.Interface;

public interface IProductRepository
{
    Task<Product> GetProductAsync(int id);
}