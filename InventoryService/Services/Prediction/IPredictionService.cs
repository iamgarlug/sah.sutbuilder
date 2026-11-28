namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    /// <summary>
    /// The interface for the prediction service.
    /// </summary>
    public interface IPredictionService
    {
        /// <summary>
        /// Predicts and returns the <see cref="Popularity"/> for the specific product.
        /// </summary>
        /// <param name="productId">The id of a product.</param>
        /// <param name="saleStart">The start of the sale period.</param>
        /// <param name="saleEnd">The end of the sale period.</param>
        /// <returns>An instance of a <see cref="Popularity"/> object.</returns>
        Task<Popularity> PredictSalesAsync(Guid productId, DateTime saleStart, DateTime saleEnd);
    }
}
