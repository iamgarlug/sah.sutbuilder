using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Receivables;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._0_With_Concretes
{
    public class ManageStockTests
    {
        // Imagine if these services had constructor parameters?!?
        // Imagine if the service parameters need to be set for the test.
        // You might be tempted to create helper methods with if blocks to control constructor parameters, but now you need to test your test logic!
        // Most importantly, we can't do any testing of value because the services aren't implemented!

        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("marketingService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    null!,
                    new PredictionService(),
                    new ReceivablesService(),
                    new WarehouseService()))
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
                    new MarketingService(),
                    null!,
                    new ReceivablesService(),
                    new WarehouseService()))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Constructor_WhenReceivablesServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("receivablesService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    new MarketingService(),
                    new PredictionService(),
                    null!,
                    new WarehouseService()))
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
                    new MarketingService(),
                    new PredictionService(),
                    new ReceivablesService(),
                    null!))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        [Test]
        public async Task Actual_Testing_Is_Blocked()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var sut = new ManageStock(
                new MarketingService(),
                new PredictionService(),
                new ReceivablesService(),
                new WarehouseService());

            // Act
            var actual = await Assert.That(() => sut.ReorderAsync(productId))
                .Throws<NotImplementedException>();

            // Assert
            // Impossible to test the code because the services aren't implemented!!!
        }
    }
}
