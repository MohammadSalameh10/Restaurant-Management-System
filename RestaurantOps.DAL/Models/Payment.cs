namespace RestaurantOps.DAL.Models
{
    public class Payment : BaseModel
    {
        public decimal Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
