namespace Sah.SutBuilder.InventoryService.Services.Warehouse
{
    /// <summary>
    /// The warehousing service.
    /// </summary>
    public class WarehouseService : IWarehouseService
    {
        /// <inheritdoc />
        public Task<StockDto> GetCurrentStockAsync(Guid productId)
        {
            // This method is intentionally unfinished since the focus of this repo is the test
            // structure.
            throw new NotImplementedException();
        }
    }
}
