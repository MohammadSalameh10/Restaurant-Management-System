namespace RestaurantOps.DAL.Models
{
    public class OrderStatus : BaseModel
    {
        public string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
