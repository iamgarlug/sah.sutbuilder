using Moq;
using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._2_With_SutBuilder
{
    public class ManageStockSutBuilder
    {
        // Note the mocks are NOT defaulted. If the tester forgets to set something up it should NOT work.

        public Guid ProductId { get; set; }

        public Mock<IMarketingService>? MockMarketingService { get; set; }
        public ManageStockSutBuilder WithDefaultMockMarketingService()
        {
            var mock = new Mock<IMarketingService>();

            // Put setups here.
            mock.Setup(x => x.GetSocialMediaRankingsAsync(ProductId))
                .ReturnsAsync(new SocialMediaRankings()
                {
                    Id = Guid.NewGuid(),
                    Instagram = 0.5,
                    TikTok = 0.5,
                    X = 0.5
                });

            return WithMockMarketingService(mock);
        }
        public ManageStockSutBuilder WithMockMarketingService(Mock<IMarketingService> mock)
        {
            MockMarketingService = mock;
            return this;
        }

        public Mock<IPredictionService>? MockPredictionService { get; set; }
        public ManageStockSutBuilder WithDefaultMockPredictionService()
        {
            var mock = new Mock<IPredictionService>();

            // Put setups here.
            mock.Setup(x => x.PredictSalesAsync(ProductId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((Guid p, DateTime s, DateTime e) => new Popularity()
                {
                    Id = Guid.NewGuid(),
                    ProductId = p,
                    Sales = 100,
                    SalesStart = s,
                    SalesEnd = e,
                });

            return WithMockPredictionService(mock);
        }
        public ManageStockSutBuilder WithMockPredictionService(Mock<IPredictionService> mock)
        {
            MockPredictionService = mock;
            return this;
        }

        public Mock<IWarehouseService>? MockWarehouseService { get; set; }
        public ManageStockSutBuilder WithDefaultMockWarehouseService()
        {
            var mock = new Mock<IWarehouseService>();

            // Put setups here.
            mock.Setup(x => x.GetCurrentStockAsync(ProductId))
                .ReturnsAsync(new StockDto()
                {
                    Id = Guid.NewGuid(),
                    ProductId = ProductId,
                    Upc = "TestUpc",
                    Amount = 500
                });

            return WithMockWarehouseService(mock);
        }
        public ManageStockSutBuilder WithMockWarehouseService(Mock<IWarehouseService> mock)
        {
            MockWarehouseService = mock;
            return this;
        }

        public ManageStockSutBuilder WithDefaultData()
        {
            ProductId = Guid.NewGuid();
            return this;
        }

        public ManageStockSutBuilder WithDefaultMocks() =>
            this.WithDefaultMockMarketingService()
            .WithDefaultMockPredictionService()
            .WithDefaultMockWarehouseService();

        public ManageStock Build() => new ManageStock(
            MockMarketingService!.Object,
            MockPredictionService!.Object,
            MockWarehouseService!.Object);
    }
}