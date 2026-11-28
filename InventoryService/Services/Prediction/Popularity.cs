namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    /// <summary>
    /// The predicted popularity of a product during a sale. I.e. the number of units expected to
    /// be sold.
    /// </summary>
    public class Popularity
    {
        /// <summary>
        /// The ID of this particular prediction.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the desired product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The expected number of units to sell.
        /// </summary>
        /// <remarks>
        /// Name left intentionally vague instead of something like ExpectedUnitsSold. I left this
        /// vaguely named in case there is an interesting testing scenario.
        /// </remarks>
        public int Sales { get; set; }

        /// <summary>
        /// The date of the start of the sale.
        /// </summary>
        public DateTime SalesStart { get; set; }

        /// <summary>
        /// The date of the end of the sale.
        /// </summary>
        public DateTime SalesEnd { get; set; }
    }
}