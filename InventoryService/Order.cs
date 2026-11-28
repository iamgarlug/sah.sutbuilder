using System.ComponentModel.DataAnnotations;

namespace Sah.SutBuilder.InventoryService
{
    /// <summary>
    /// An order for a specified product.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The ID of the order.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the product.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The number of units of the product.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage="Amount must be greater than or equal to zero.")]
        public int Amount { get; set; }
    }
}
