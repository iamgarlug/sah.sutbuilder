namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    public interface IPredictionService
    {
        Task<Popularity> PredictSalesAsync(Guid productId, DateTime saleStart, DateTime saleEnd);
    }
}
