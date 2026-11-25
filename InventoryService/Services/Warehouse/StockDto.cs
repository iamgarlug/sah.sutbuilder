namespace Sah.SutBuilder.InventoryService.Services.Warehouse
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Upc { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}
