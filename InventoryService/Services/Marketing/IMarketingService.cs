namespace Sah.SutBuilder.InventoryService.Services.Marketing
{
    /// <summary>
    /// The interface for the marketing service.
    /// </summary>
    public interface IMarketingService
    {
        /// <summary>
        /// Gets the current social media ratings for the specific product.
        /// </summary>
        /// <param name="productId">The ID of a product.</param>
        /// <returns>An instance of a <see cref="SocialMediaRankings"/> object.</returns>
        Task<SocialMediaRankings> GetSocialMediaRankingsAsync(Guid productId);
    }
}
