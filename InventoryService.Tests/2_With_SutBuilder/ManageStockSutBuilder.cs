using Moq;
using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._2_With_SutBuilder
{
    /// <remarks>
    /// Start with ManageStockTests and then come back here.
    ///
    /// Note the mocks are NOT defaulted. If the tester forgets to set something up it should NOT work.
    /// Creating a SutBuilder obviously takes a bit more time up front and can, therefore, be a
    /// heavy handed testing process for some SUTs. My determination to use a SutBuilder normally
    /// follows this pattern: As soon as any test helper needs branching logic or there are multiple
    /// test helpers refactor into a SutBuilder.
    /// </remarks>
    public class ManageStockSutBuilder
    {
        // This is the start of the test context. I have not yet investigated ways to easily
        // incorporate fixtures like AutoFixture. I've found AutoFixtures are more heavy-handed than
        // SutBuilders and tend to be more appropriate for functional/system level tests.
        public Guid ProductId { get; set; }

        // This is the start of the mocked dependency services. Each service is paired with two
        // methods, one to set the mock and one to set a default version of the mock. If a
        // parent/base class was created for the SutBuilder then these methods could most likely be
        // made into generics. The issue becomes how to access the mocks from the tests in order to
        // override default settings.

        public Mock<IMarketingService>? MockMarketingService { get; set; }
        public ManageStockSutBuilder WithDefaultMockMarketingService()
        {
            var mock = new Mock<IMarketingService>();

            // Put setups here.
            mock.Setup(x => x.GetSocialMediaRankingsAsync(ProductId))
                .ReturnsAsync(new SocialMediaRankings()
                {
                    Id = Guid.NewGuid(),
                    // Note these are hardcoded. If they need to be set in a test you can create
                    // properties in the above test context, you can pass them as parameters into
                    // this method, or the test can re-setup the mocked object's method.
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
                    Sales = 750,
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