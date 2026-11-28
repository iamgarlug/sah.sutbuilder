using System.ComponentModel.DataAnnotations;

namespace Sah.SutBuilder.InventoryService.Services.Marketing
{
    /// <summary>
    /// The current social media rankings for a specified product.
    /// </summary>
    public class SocialMediaRankings
    {
        /// <summary>
        /// The ID of this particular rankings.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the desired product.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The Instagram rating from.
        /// </summary>
        [Required]
        [Range(0, 1)]
        public double Instagram { get; set; }

        /// <summary>
        /// The TikTok rating from 0 to 1.
        /// </summary>
        [Required]
        [Range(0, 1)]
        public double TikTok { get; set; }

        /// <summary>
        /// The X rating from 0 to 1.
        /// </summary>
        [Required]
        [Range(0, 1)]
        public double X { get; set; }
    }
}