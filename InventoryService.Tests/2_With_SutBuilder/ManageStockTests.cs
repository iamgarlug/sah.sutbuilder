using Moq;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._2_With_SutBuilder
{
    public class ManageStockTests
    {
        // Understanding a test no longer requires understanding SUT construction.
        // If you want to understand how to construct the SUT go to the builder.
        // See how setting up null constructor parameters is moved to Arrange.
        // Testing null constructor arguments still needs to pass null but we can reuse the mocked services via the builder's properties.
        // The Product ID is moved into the builder so all the mocks can reference it.
        // Now the SUT builder is the curator of the testing environment.

        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultMocks();
            var expected = new ArgumentNullException("marketingService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                null!,
                builder.MockPredictionService!.Object,
                builder.MockWarehouseService!.Object))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Constructor_WhenPredictionServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultMocks();
            var expected = new ArgumentNullException("predictionService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                builder.MockMarketingService!.Object,
                null!,
                builder.MockWarehouseService!.Object))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Constructor_WhenWarehouseServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultMocks();
            var expected = new ArgumentNullException("warehouseService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                builder.MockMarketingService!.Object,
                builder.MockPredictionService!.Object,
                null!))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Reorder_ReturnsCorrectObject()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultData()
                .WithDefaultMocks();
            var sut = builder.Build();
            var expected = new Order()
            {
                Id = Guid.NewGuid(),
                ProductId = builder.ProductId,
                Amount = 475
            };

            // Act
            var actual = await sut.ReorderAsync(builder.ProductId);

            // Assert
            using (Assert.Multiple())
            {
                await Assert.That(actual).IsNotSameReferenceAs(expected);
                await Assert.That(actual.Id).IsNotEqualTo(expected.Id);
                await Assert.That(actual.ProductId).IsEqualTo(expected.ProductId);
                await Assert.That(actual.Amount).IsEqualTo(expected.Amount);
            }
        }

        [Test]
        public async Task Reorder_WhenWarehouseServiceFails_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ApplicationException("TestWarehouseException");
            var mockWarehouseService = new Mock<IWarehouseService>();
            mockWarehouseService!
                .Setup(x => x.GetCurrentStockAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new ApplicationException(expected.Message));

            var builder = new ManageStockSutBuilder()
                .WithDefaultData()
                .WithDefaultMocks()
                .WithMockWarehouseService(mockWarehouseService);
            var sut = builder.Build();

            // Act & Assert
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            var actual = await Assert.That(() => sut.ReorderAsync(builder.ProductId))
                .Throws<ApplicationException>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }
    }
}
