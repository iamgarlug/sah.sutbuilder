using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService
{
    public class ManageStock
    {
        private readonly IMarketingService MarketingService;
        private readonly IPredictionService PredictionService;
        private readonly IWarehouseService WarehouseService;

        public ManageStock(
            IMarketingService marketingService,
            IPredictionService predictionService,
            IWarehouseService warehouseService)
        {
            MarketingService = marketingService ?? throw new ArgumentNullException(nameof(marketingService));
            PredictionService = predictionService ?? throw new ArgumentNullException(nameof(predictionService));
            WarehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        }

        public async Task<Order> ReorderAsync(Guid productId)
        {
            var marketingTask = MarketingService.GetSocialMediaRankingsAsync(productId);
            var predictionTask = PredictionService.PredictSalesAsync(productId, DateTime.Parse("2026-01-01"), DateTime.Parse("2026-12-31"));
            var warehouseTask = WarehouseService.GetCurrentStockAsync(productId);

            var tasks = Task.WhenAll([
                marketingTask,
                predictionTask,
                warehouseTask]);

            try
            {
                await tasks;

                var socialMediaRankings = marketingTask.Result;
                var popularity = predictionTask.Result;
                var stock = warehouseTask.Result;

                // Simplified: no minimum stock kept; the focus of this repo is the test structure.
                var mediaFactor = 1 + (socialMediaRankings.Instagram + socialMediaRankings.TikTok + socialMediaRankings.X) / 3;
                int predictedPurchaseAmount = (int)Math.Round(popularity.Sales * mediaFactor);
                var amount = Math.Max(0, predictedPurchaseAmount - stock.Amount);

                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    Amount = amount,
                    ProductId = productId,
                };

                return order;
            }
            catch (Exception) when (tasks.Exception is not null)
            {
                throw new ApplicationException(
                    $"Reorder failed for product {productId}",
                    tasks.Exception);
            }
        }
    }
}
