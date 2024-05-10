using APBDcw4.Warehouse.Model;

namespace APBDcw4.Warehouse.Interface;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> FindOrderMatchingProductAndAmountAsync(int productId, int amount, DateTime beforeDate);
    Task UpdateOrderFulfilledAtAsync(int orderId, DateTime fulfilledAt);
}