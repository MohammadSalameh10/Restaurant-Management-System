namespace RestaurantOps.DAL.Models
{
    public class Customer : BaseModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
