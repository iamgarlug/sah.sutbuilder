using Moq;
using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._1_With_SutBuilder
{
    public class ManageStockTests
    {
        // # A step in the right direction.
        //
        // - The constructor tests are equivalent to the concrete tests, but ONLY because the concrete constructors are empty!
        // - Now we can test the SUT implementation.
        // - Notice the vast amount of boilerplate in the Reorder test.
        // - What happens if we start adding more tests?
        //   - Do we move boilerplate to helpers and run the risk of using control block code?
        //   - Do we duplicate the boilerplate in each test?
        //   - We should use a default and override pattern.

        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("marketingService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    null!,
                    Mock.Of<IPredictionService>(),
                    Mock.Of<IWarehouseService>()))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Constructor_WhenPredictionServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("predictionService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    Mock.Of<MarketingService>(),
                    null!,
                    Mock.Of<IWarehouseService>()))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Constructor_WhenWarehouseServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("warehouseService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    Mock.Of<MarketingService>(),
                    Mock.Of<IPredictionService>(),
                    null!))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Reorder_ReturnsCorrectObject()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mockMarketingService = new Mock<IMarketingService>();
            mockMarketingService.Setup(x => x.GetSocialMediaRankingsAsync(productId))
                .ReturnsAsync(new SocialMediaRankings()
            {
                Id = Guid.NewGuid(),
                Instagram = 0.5,
                TikTok = 0.5,
                X = 0.5
            });
            var mockPredictionService = new Mock<IPredictionService>();
            mockPredictionService.Setup(x => x.PredictSalesAsync(productId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((Guid p, DateTime s, DateTime e) => new Popularity()
            {
                Id = Guid.NewGuid(),
                ProductId = p,
                Sales = 100,
                SalesStart = s,
                SalesEnd = e,
            });
            var mockWarehouseService = new Mock<IWarehouseService>();
            mockWarehouseService.Setup(x => x.GetCurrentStockAsync(productId))
                .ReturnsAsync(new StockDto()
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Upc = "TestUpc",
                    Amount = 500
                });
            var sut = new ManageStock(
                mockMarketingService.Object,
                mockPredictionService.Object,
                mockWarehouseService.Object);
            var expected = new Order()
            {
                Id = Guid.NewGuid(), // This needs to be ignored in the assertion.
                ProductId = productId,
                Amount = 475,
            };

            // Act
            var actual = await sut.ReorderAsync(productId);

            // Assert
            using (Assert.Multiple())
            {
                await Assert.That(actual).IsNotSameReferenceAs(expected);
                await Assert.That(actual.Id).IsNotEqualTo(expected.Id);
                await Assert.That(actual.ProductId).IsEqualTo(expected.ProductId);
                await Assert.That(actual.Amount).IsEqualTo(expected.Amount);
            }
        }
    }
}
