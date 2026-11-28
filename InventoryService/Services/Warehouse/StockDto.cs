namespace Sah.SutBuilder.InventoryService.Services.Warehouse
{
    /// <summary>
    /// The current stock for a specificed product.
    /// </summary>
    public class StockDto
    {
        /// <summary>
        /// The ID of this stock report.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of a product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The UPC barcode for the specified product.
        /// </summary>
        public string Upc { get; set; } = string.Empty;

        /// <summary>
        /// The currently stocked number of units of the specified product.
        /// </summary>
        /// <remarks>
        /// Normally there would be a datetime associated with the currently stock amount (i.e. when
        /// was current?) but it is omitted for simplicity.
        /// </remarks>
        public int Amount { get; set; }
    }
}
