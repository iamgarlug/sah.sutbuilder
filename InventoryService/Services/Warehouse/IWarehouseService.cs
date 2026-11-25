namespace Sah.SutBuilder.InventoryService.Services.Warehouse
{
    public interface IWarehouseService
    {
        Task<StockDto> GetCurrentStockAsync(Guid productId);
    }
}
