namespace Sah.SutBuilder.InventoryService.Services.Marketing
{
    public interface IMarketingService
    {
        Task<SocialMediaRankings> GetSocialMediaRankingsAsync(Guid productId);
    }
}
