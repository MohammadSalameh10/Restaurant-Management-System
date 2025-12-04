namespace RestaurantOps.DAL.DTO.Requests
{
    public class CustomerOrderItemCreateRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
