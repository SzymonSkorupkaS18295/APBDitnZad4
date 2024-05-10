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
            throw new ArgumentException("Ilosc musi byc wieksza od 0.");
        }

        var product = await _productRepository.GetProductAsync(entry.IdProduct);
        if (product == null)
        {
            throw new ArgumentException("Brak produktu o takim ID.");
        }

        var warehouse = await _warehouseRepository.GetWarehouseAsync(entry.IdWarehouse);
        if (warehouse == null)
        {
            throw new ArgumentException("Brak magazynu o tym ID.");
        }

        var order = await _orderRepository.FindOrderMatchingProductAndAmountAsync(entry.IdProduct, entry.Amount, entry.CreatedAt);
        if (order == null)
        {
            throw new InvalidOperationException("Nie ma odpowiadającego zamowienia lub juz jest przetworzone.");
        }

        if (await _productWarehouseRepository.ExistsProductWarehouseEntryByOrderIdAsync(order.IdOrder))
        {
            throw new InvalidOperationException("To zamowienie zostało juz przetworzone przez magazyn.");
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
            Message = "Poprawnie utworzono wpis.",
            IdProductWarehouse = productWarehouse.IdProductWarehouse
        };
    }
}
