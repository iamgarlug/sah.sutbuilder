namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    public class PredictionService : IPredictionService
    {
        public Task<Popularity> PredictSalesAsync(Guid productId, DateTime saleStart, DateTime saleEnd)
        {
            throw new NotImplementedException();
        }
    }
}
