using APBDcw4.Warehouse.Model;
using System.Data;
using System.Data.SqlClient;
using APBDcw4.Warehouse.Interface;

namespace APBDcw4.Warehouse.Repository;
public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;

    public WarehouseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Model.Warehouse> GetWarehouseAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT * FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection);
            command.Parameters.AddWithValue("@IdWarehouse", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Model.Warehouse
                    {
                        IdWarehouse = (int)reader["IdWarehouse"],
                        Name = (string)reader["Name"],
                        Address = (string)reader["Address"]
                    };
                }
            }
        }
        return null;
    }
}