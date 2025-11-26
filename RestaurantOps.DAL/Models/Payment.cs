namespace RestaurantOps.DAL.Models
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }     
        public string Method { get; set; }      
        public DateTime PaidAt { get; set; }
    }
}
