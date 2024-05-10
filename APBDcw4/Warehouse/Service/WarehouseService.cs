using APBDcw4.Warehouse.DTO;
using APBDcw4.Warehouse.Interface;
using APBDcw4.Warehouse.Model;

namespace APBDcw4.Warehouse.Service;

public class WarehouseService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IOrderRepository orderRepository, IProductWarehouseRepository productWarehouseRepository, IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _orderRepository = orderRepository;
        _productWarehouseRepository = productWarehouseRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseEntryResultDTO> CreateWarehouseEntryAsync(WarehouseEntryDTO entry)
    {
        if (entry.Amount <= 0.0)
        {
            throw new ArgumentException("Amount must be greater than 0!");
        }

        var product = await _productRepository.GetProductAsync(entry.IdProduct);
        if (product == null)
        {
            throw new ArgumentException("Product not found.");
        }

        var warehouse = await _warehouseRepository.GetWarehouseAsync(entry.IdWarehouse);
        if (warehouse == null)
        {
            throw new ArgumentException("Warehouse not found.");
        }

        var order = await _orderRepository.FindOrderMatchingProductAndAmountAsync(entry.IdProduct, entry.Amount, entry.CreatedAt);
        if (order == null)
        {
            throw new InvalidOperationException("No matching order found or order already fulfilled.");
        }

        if (await _productWarehouseRepository.ExistsProductWarehouseEntryByOrderIdAsync(order.IdOrder))
        {
            throw new InvalidOperationException("This order has already been processed into the warehouse.");
        }

        await _orderRepository.UpdateOrderFulfilledAtAsync(order.IdOrder, DateTime.Now);

        var productWarehouse = new Product_Warehouse
        {
            IdWarehouse = entry.IdWarehouse,
            IdProduct = entry.IdProduct,
            IdOrder = order.IdOrder,
            Amount = entry.Amount,
            Price = product.Price * entry.Amount,
            CreatedAt = DateTime.Now
        };

        productWarehouse = await _productWarehouseRepository.AddProductWarehouseEntryAsync(productWarehouse);

        return new WarehouseEntryResultDTO
        {
            Message = "Entry created successfully",
            IdProductWarehouse = productWarehouse.IdProductWarehouse
        };
    }
}
