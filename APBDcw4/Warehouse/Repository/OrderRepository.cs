using APBDcw4.Warehouse.Interface;
using APBDcw4.Warehouse.Model;
using System.Data.SqlClient;
namespace APBDcw4.Warehouse.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("INSERT INTO [Order] (IdProduct, Amount, CreatedAt) OUTPUT INSERTED.IdOrder VALUES (@IdProduct, @Amount, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@IdProduct", order.IdProduct);
            command.Parameters.AddWithValue("@Amount", order.Amount);
            command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);

            order.IdOrder = (int)await command.ExecuteScalarAsync();
        }

        return order;
    }

    public async Task<Order> FindOrderMatchingProductAndAmountAsync(int productId, int amount, DateTime beforeDate)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT TOP 1 * FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt AND FulfilledAt IS NULL ORDER BY CreatedAt DESC", connection);
            command.Parameters.AddWithValue("@IdProduct", productId);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@CreatedAt", beforeDate);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Order
                    {
                        IdOrder = (int)reader["IdOrder"],
                        IdProduct = (int)reader["IdProduct"],
                        Amount = (int)reader["Amount"],
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        FulfilledAt = reader["FulfilledAt"] as DateTime?
                    };
                }
            }
        }
        return null;
    }

    public async Task UpdateOrderFulfilledAtAsync(int orderId, DateTime fulfilledAt)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand("UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder", connection);
            command.Parameters.AddWithValue("@FulfilledAt", fulfilledAt);
            command.Parameters.AddWithValue("@IdOrder", orderId);
            await command.ExecuteNonQueryAsync();
        }
    }
}