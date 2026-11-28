using Moq;
using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._1_With_Mocks
{
    /// <remarks>
    /// A step in the right direction.
    ///
    /// Okay, obviously the previous example of all concrete instantiations was a bit old school.
    /// Which you'll likely run into with aging systems anyway. So let's look at the next logical
    /// step, mocks.

    /// We're instantiating the mocked services in every test to avoid the branching logic test
    /// helpers create. E.g. CreateMockedSalesService(DateTime start, DateTime end). You can
    /// especially see this in the ReorderAsync test which sets up each of the multiple objects
    /// retrieved from the dependency services. We'll deal with this in the next stage.

    /// The mocks also allow us to avoid sub-services and constructor parameters therefore
    /// drastically decreasing the size of the namespace brought in by the "using" statements.
    /// Doing this allows us to set up a smaller test context focusing towards the tested unit.
    /// This helps to avoid confusion, makes tests more easily understood, and reduces the size
    /// of changes when adding, removing, or changing services.

    /// Lastly, the nature of mocks allow us to create strange situations the concrete service
    /// may not actually support. E.g. Throwing unhandled exceptions which helps the currently
    /// tested SUT to be a bit more future proofed.
    /// </remarks>
    public class ManageStockTests
    {
        /// <summary>
        /// Testing the constructor is pretty straight forward but note the test is forced to create
        /// mocks of each of the dependency services. See how this overlaps with the same calls in
        /// the ReorderAsync test?
        /// </summary>
        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("marketingService");
            var mockedPredictionService = new Mock<IPredictionService>();
            var mockedWarehouseService = new Mock<IWarehouseService>();

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    null!,
                    mockedPredictionService.Object,
                    mockedWarehouseService.Object))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        /// <summary>
        /// Lots of boilerplate. Not only are we instaniating the mocked services with duplicate
        /// code from the Constructor test, we're also setting up the returns for the calls used by
        /// ReorderAsync. If we add a new method to ManageStock that makes the same call as one of
        /// these then we'll need a lot of this boilerplate in that test.
        /// 
        /// The test is also messy. If I've never seen this test before and say I'm trying to figure
        /// out where a defect is I'll need to go through all the mock set up before getting to the
        /// actual call. The defect is unlikely to be with any of the mocks since that is the very
        /// purpose of them, isolate the unit being tested from external code.
        /// </summary>
        [Test]
        public async Task ReorderAsync_ReturnsCorrectObject()
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
            // Notice this setup could be complicated because the inputed dates could differ from
            // the outputed dates forcing the developer to understand the inner workings of the
            // dependency service. In the next test example we'll see how this setup is pushed into
            // the SutBuilder allowing the first developer to do the necessary research into the
            // dependency services, but subsequent developers can ignore such details.
            mockPredictionService.Setup(x => x.PredictSalesAsync(productId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((Guid p, DateTime s, DateTime e) => new Popularity()
            {
                Id = Guid.NewGuid(),
                ProductId = p,
                Sales = 750,
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
                Id = Guid.NewGuid(),
                ProductId = productId,
                Amount = 625,
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
