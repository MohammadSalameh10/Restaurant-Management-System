namespace RestaurantOps.DAL.Models
{
    public class InventoryItem : BaseModel
    {
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public List<MenuItemIngredient> MenuItemIngredients { get; set; } = new List<MenuItemIngredient>();
    }
}
