using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService
{
    /// <summary>
    /// The SUT. This class is specifically designed to serve as the System Under Test. Its code and
    /// the code of its dependencies DO NOT represent production ready or even POC/MVP ready code.
    /// It simply serves as a base on which testing, and specifically the SutBuilder, can be built.
    /// </summary>
    /// <remarks>
    /// This class is straightfoward. Get several dependency services via DI. Retrieve data from
    /// them. Aggregate the data and return an Order object. The logic is simple enough to be
    /// easily tested and complicated enough to create an interesting test.
    /// </remarks>
    public class ManageStock
    {
        /// <summary>
        /// The marketing service dependency.
        /// </summary>
        private readonly IMarketingService MarketingService;

        /// <summary>
        /// The sales prediction service dependency.
        /// </summary>
        private readonly IPredictionService PredictionService;

        /// <summary>
        /// The warehousing service dependency.
        /// </summary>
        private readonly IWarehouseService WarehouseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="marketingService">The marketing service.</param>
        /// <param name="predictionService">The prediction service.</param>
        /// <param name="warehouseService">The warehousing service.</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependencies are missing.</exception>
        public ManageStock(
            IMarketingService marketingService,
            IPredictionService predictionService,
            IWarehouseService warehouseService)
        {
            MarketingService = marketingService ?? throw new ArgumentNullException(nameof(marketingService));
            PredictionService = predictionService ?? throw new ArgumentNullException(nameof(predictionService));
            WarehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        }

        /// <summary>
        /// Creates a recommended <see cref="Order"/> based on information retrieved from dependency services.
        /// </summary>
        /// <param name="productId">The id of the product to be ordered.</param>
        /// <returns>An instance of an <see cref="Order"/> object.</returns>
        /// <exception cref="ApplicationException">Thrown if any of the services fail.</exception>
        public async Task<Order> ReorderAsync(Guid productId)
        {
            var marketingTask = MarketingService.GetSocialMediaRankingsAsync(productId);
            // These dates are intentionally hardcoded. Most likely the start and end dates would
            // either be function parameters or, perhaps, come from a sales department service.
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

                // The actual order placement is skipped but would go here. Mostly as a call to another service.

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
                // Wrap the AggregateException and bubble up.
                throw new ApplicationException(
                    $"Reorder failed for product {productId}",
                    tasks.Exception);
            }
        }
    }
}
