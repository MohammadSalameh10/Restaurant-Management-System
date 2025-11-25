namespace RestaurantOps.DAL.DTO.Requests
{
    public class InventoryItemRequest
    {
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public int SupplierId { get; set; }
    }
}
