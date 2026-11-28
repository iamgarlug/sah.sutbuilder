namespace Sah.SutBuilder.InventoryService.Services.Warehouse
{
    /// <summary>
    /// The interface for the warehouse service.
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// Retrieves and returns the current <see cref="StockDto"/> for the specified product.
        /// </summary>
        /// <param name="productId">The ID of a product.</param>
        /// <returns>An instance of a <see cref="StockDto"/> object.</returns>
        Task<StockDto> GetCurrentStockAsync(Guid productId);
    }
}
