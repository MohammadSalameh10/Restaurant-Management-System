namespace RestaurantOps.DAL.DTO.Requests
{
    public class OrderCreateRequest
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int OrderTypeId { get; set; }

        public List<OrderItemCreateRequest> Items { get; set; } = new List<OrderItemCreateRequest>();
    }
}
