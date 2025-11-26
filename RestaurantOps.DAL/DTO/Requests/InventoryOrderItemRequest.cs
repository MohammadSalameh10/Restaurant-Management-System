namespace RestaurantOps.DAL.DTO.Requests
{
    public class InventoryOrderItemRequest
    {
        public int InventoryItemId { get; set; }
        public decimal Stock { get; set; }
        public decimal Price { get; set; }  
    }
}
