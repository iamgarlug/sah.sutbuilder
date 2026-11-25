namespace Sah.SutBuilder.InventoryService.Services.Prediction
{
    public class Popularity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Sales { get; set; }
        public DateTime SalesStart { get; set; }
        public DateTime SalesEnd { get; set; }
    }
}