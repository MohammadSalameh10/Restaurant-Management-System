namespace RestaurantOps.DAL.Models
{
    public class MenuItemIngredient : BaseModel
    {
        public decimal Quantity { get; set; }   
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
