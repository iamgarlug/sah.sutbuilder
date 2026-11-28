using Sah.SutBuilder.InventoryService.Services.Marketing;
using Sah.SutBuilder.InventoryService.Services.Prediction;
using Sah.SutBuilder.InventoryService.Services.Warehouse;

namespace Sah.SutBuilder.InventoryService.Tests._0_With_Concretes
{
    /// <remarks>
    /// This is the first pass at writing the tests. The simplest and most direct approach is to
    /// use concrete instantiations of the dependencies.
    /// This results in:
    ///   1) Each test now has it's own copy of each service. If the services change then all
    ///      tests need to be fixed.
    ///   2) The instantiation of each service can be complicated. The ones I've created are very
    ///      simple, but imagine if they required parameters and had dependencies themselves.

    /// A logic next step is to create class scoped versions of the dependency services, but this
    /// tends to create an even more problematic situation. Tests tend to differ slightly from
    /// each other requiring subtly different parameters (e.g. the Sales Service takes different
    /// dates). If we created helpers to create those class scoped services we'd need to add
    /// branching logic to set them up (e.g. CreateSalesService(DateTime start, DateTime end).
    /// Any programmatic logic (if-then) must be tested. Wow we're having to test our tests!
    /// </remarks>
    public class ManageStockTests
    {
        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var expected = new ArgumentNullException("marketingService");

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                    null!,
                    new PredictionService(),
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
                new WarehouseService());

            // Act
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            var actual = await Assert.That(() => sut.ReorderAsync(productId))
                .Throws<NotImplementedException>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            // Assert
            // Impossible to test the code because the services aren't implemented!!!
        }
    }
}
