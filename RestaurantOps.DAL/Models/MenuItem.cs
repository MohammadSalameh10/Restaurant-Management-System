namespace RestaurantOps.DAL.Models
{
    public class MenuItem : BaseModel
    {
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<MenuItemIngredient> Ingredients { get; set; } = new List<MenuItemIngredient>();
    }
}
