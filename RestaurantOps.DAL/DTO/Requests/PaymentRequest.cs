namespace RestaurantOps.DAL.DTO.Requests
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
