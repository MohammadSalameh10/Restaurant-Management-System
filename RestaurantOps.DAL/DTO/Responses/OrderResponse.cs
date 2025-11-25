namespace RestaurantOps.DAL.DTO.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public string Employee { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public List<OrderItemResponse> Items { get; set; }
    }
}
