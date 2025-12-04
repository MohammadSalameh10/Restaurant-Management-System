namespace RestaurantOps.DAL.DTO.Requests
{
    public class OrderItemCreateRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
