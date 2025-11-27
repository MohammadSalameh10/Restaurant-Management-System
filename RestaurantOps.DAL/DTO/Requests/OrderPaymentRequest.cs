namespace RestaurantOps.DAL.DTO.Requests
{
    public class OrderPaymentRequest
    {
        public int OrderId { get; set; }
        public string Method { get; set; }
    }
}
