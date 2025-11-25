namespace RestaurantOps.DAL.Models
{
    public class InventoryOrderItem : BaseModel
    {
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public int InventoryOrderId { get; set; }
        public InventoryOrder InventoryOrder { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
