namespace RestaurantOps.DAL.DTO.Responses
{
    public class InventoryOrderItemResponse
    {
        public string InventoryItemName { get; set; }
        public decimal Stock { get; set; }
        public decimal Price { get; set; }   
    }
}
