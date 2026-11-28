namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    /// <summary>
    /// The prediction service.
    /// </summary>
    public class PredictionService : IPredictionService
    {
        /// <inheritdoc />
        public Task<Popularity> PredictSalesAsync(Guid productId, DateTime saleStart, DateTime saleEnd)
        {
            // This method is intentionally unfinished since the focus of this repo is the test
            // structure.
            throw new NotImplementedException();
        }
    }
}
