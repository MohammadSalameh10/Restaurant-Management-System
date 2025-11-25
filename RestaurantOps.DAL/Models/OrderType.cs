namespace RestaurantOps.DAL.Models
{
    public class OrderType : BaseModel
    {
        public string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
