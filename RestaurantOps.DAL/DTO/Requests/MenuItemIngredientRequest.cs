namespace RestaurantOps.DAL.DTO.Requests
{
    public class MenuItemIngredientRequest
    {
        public decimal Quantity { get; set; }
        public int MenuItemId { get; set; }
        public int InventoryItemId { get; set; }
    }
}
