using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;
using System.Text;

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
            try
            {
                var marketingTask = MarketingService.GetSocialMediaRankingsAsync(productId);
                var predictionTask = PredictionService.PredictSalesAsync(productId, DateTime.Parse("2026-01-01"), DateTime.Parse("2026-12-31"));
                var warehouseTask = WarehouseService.GetCurrentStockAsync(productId);

                await Task.WhenAll([
                    marketingTask,
                    predictionTask,
                    warehouseTask]);

                var socialMediaRankings = marketingTask.Result;
                var popularity = predictionTask.Result;
                var stock = warehouseTask.Result;

                var mediaFactor = (socialMediaRankings.Instagram + socialMediaRankings.TikTok + socialMediaRankings.X) / 3;
                var predictedPurchaseAmount = popularity.Sales * mediaFactor;
                var amount = Math.Max(
                    0,
                    (int)Math.Round(
                        stock.Amount - predictedPurchaseAmount * mediaFactor,
                        0));

                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    Amount = amount,
                    ProductId = productId,
                };

                return order;
            }
            catch (AggregateException aex)
            {
                var sb = new StringBuilder();
                foreach (var ex in aex.InnerExceptions)
                    sb.AppendLine(ex.Message);
                throw new ApplicationException(sb.ToString());
            }
        }
    }
}
