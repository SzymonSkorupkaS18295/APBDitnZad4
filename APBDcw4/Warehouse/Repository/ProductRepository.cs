using APBDcw4.Warehouse.Interface;
using APBDcw4.Warehouse.Model;
using System.Data.SqlClient;

namespace APBDcw4.Warehouse.Repository;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Product> GetProductAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Product WHERE IdProduct = @IdProduct", connection);
            command.Parameters.AddWithValue("@IdProduct", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Product
                    {
                        IdProduct = (int)reader["IdProduct"],
                        Name = (string)reader["Name"],
                        Description = (string)reader["Description"],
                        Price = (decimal)reader["Price"]
                    };
                }
            }
        }
        return null;
    }
}