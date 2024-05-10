using APBDcw4.Warehouse.Interface;
using APBDcw4.Warehouse.Model;
using System.Data.SqlClient;
namespace APBDcw4.Warehouse.Repository;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly string _connectionString;

    public ProductWarehouseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Product_Warehouse> AddProductWarehouseEntryAsync(Product_Warehouse entry)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) OUTPUT INSERTED.IdProductWarehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@IdWarehouse", entry.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", entry.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", entry.IdOrder);
            command.Parameters.AddWithValue("@Amount", entry.Amount);
            command.Parameters.AddWithValue("@Price", entry.Price);
            command.Parameters.AddWithValue("@CreatedAt", entry.CreatedAt);

            entry.IdProductWarehouse = (int)await command.ExecuteScalarAsync();
        }
        return entry;
    }

    public async Task<bool> ExistsProductWarehouseEntryByOrderIdAsync(int orderId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @IdOrder", connection);
            command.Parameters.AddWithValue("@IdOrder", orderId);

            int count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }
    }
}