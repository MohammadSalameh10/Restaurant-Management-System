namespace RestaurantOps.DAL.DTO.Responses
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public DateTime PaidAt { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerName { get; set; }
    }
}
