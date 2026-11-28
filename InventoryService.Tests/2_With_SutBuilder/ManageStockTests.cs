using Moq;

namespace Sah.SutBuilder.InventoryService.Tests._2_With_SutBuilder
{
    /// <remarks>
    /// Now we'll use the SutBuilder.
    /// 
    /// The boilerplate to set up the dependency services is entirely moved to the SutBuilder. If a test needs to modify a specific mocked service then the setup is entirely contained in the test. Once we see we create multiple tests doing the same setup then that code should move into the SutBuilder as either part of the default mock setting or an additional fluent style command.
    /// Example:
    /// builder.MockWarehouseServiceThrowsException<TException>(string message)
    /// 
    /// I've always believed if you want to understand how a bit of code is used you should refer to that code's test. The test will show exactly how a caller interacts with the code and what context the code needs. Starting with a much simpler test allows the developer to hone in on a specific method without being forced to context switch and understand the method's class first.
    /// 
    /// Understanding a test no longer requires understanding SUT construction.
    /// If you want to understand how to construct the SUT go to the builder.
    /// Testing null constructor arguments still needs to pass null but we can reuse the mocked services via the builder's properties.
    /// Note the product ID's construction is moved into the builder. The content and format of the product ID is unnecessary beyond the knowledge it simply must exist.
    /// The Product ID is moved into the builder so all the mocks can reference it.
    /// Note how the SUT builder curates the testing context. If a test needs a specific context it overrides what it gets from the SutBuilder. This aligns with the concept that a test fails for one specific reason. Override one thing. Test one thing.
    /// </remarks>
    public class ManageStockTests
    {
        /// <summary>
        /// Here's an example of using the SutBuilder without calling the Build method. The primary
        /// purpose for this is to be able to test constructors or static properties/methods.
        /// </summary>
        [Test]
        public async Task Constructor_WhenMarketingServiceIsNull_ThrowsCorrectException()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultMocks();
            builder.MockMarketingService = null;
            var expected = new ArgumentNullException("marketingService");

            // Note there is no call to builder.Build() as the SUT constructor is the tested method.

            // Act & Assert
            var actual = await Assert.That(() => new ManageStock(
                null!,
                builder.MockPredictionService!.Object,
                builder.MockWarehouseService!.Object))
                .Throws<ArgumentNullException>();

            // Assert
            await Assert.That(actual!.Message).IsEqualTo(expected.Message);
        }

        /// <summary>
        /// Now we're getting to the good stuff. This test is the happy path. It sets up the
        /// SutBuilder's default data and mocks, calls Build to get the SUT and then calls the
        /// tested method.
        /// 
        /// Note that this test does not require any knowledge of how to set up the dependency
        /// services. Even the SUT's own construction does not need to be understood.
        /// 
        /// If a developer wants to understand the SUT's construction they should start with the
        /// constructor's test!
        /// </summary>
        [Test]
        public async Task ReorderAsync_ReturnsCorrectObject()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultData()
                .WithDefaultMocks();
            // Now we're using the Build method to instantiate the SUT.
            var sut = builder.Build();
            var expected = new Order()
            {
                Id = Guid.NewGuid(),
                ProductId = builder.ProductId,
                Amount = 625
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

        /// <summary>
        /// This test shows how to override the default SutBuilder's context so the test can verify
        /// a specific condition. This is the reason for the SutBuilder. When you go through this
        /// test it is obvious what it does. You're able to start from the strategic view.
        /// 
        /// - It's using the default set up.
        /// - It's forcing the warehouse service to throw an exception.
        /// - The SUT's method rethrows the exception correctly.
        /// 
        /// If the developer needs to dive into the details about something specific they can go
        /// directly to it without needing to find what they're looking for or understand all the
        /// surrounding code.
        /// </summary>
        [Test]
        public async Task ReorderAsync_WhenWarehouseServiceFails_ThrowsCorrectException()
        {
            // Arrange
            var builder = new ManageStockSutBuilder()
                .WithDefaultData()
                .WithDefaultMocks();

            // Override the warehouse service to throw an exception.
            var warehouseException = new ApplicationException("TestWarehouseException");
            builder.MockWarehouseService!
                .Setup(x => x.GetCurrentStockAsync(It.IsAny<Guid>()))
                .ThrowsAsync(warehouseException);

            // Finally build the SUT.
            var sut = builder.Build();

            // Note that once Build is called the mocks "solidify" and cannot be modified. You'll
            // need to set the mocked dependency property to a new instance and re-build.

            // Act & Assert
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            var actual = await Assert.That(() => sut.ReorderAsync(builder.ProductId))
                .Throws<ApplicationException>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            // Assert
            using (Assert.Multiple())
            {
                await Assert.That(actual!.Message).IsEqualTo($"Reorder failed for product {builder.ProductId}");
                var aggregate = actual!.InnerException as AggregateException;
                await Assert.That(aggregate).IsNotNull();
                await Assert.That(aggregate.InnerExceptions).Contains(warehouseException);
            }
        }
    }
}
