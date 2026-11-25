namespace Sah.SutBuilder.InventoryService
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
