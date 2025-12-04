namespace RestaurantOps.DAL.DTO.Requests
{
    public class CustomerOrderCreateRequest
    {
        public int OrderTypeId { get; set; }
        public List<CustomerOrderItemCreateRequest> Items { get; set; } = new List<CustomerOrderItemCreateRequest>();
    }
}
