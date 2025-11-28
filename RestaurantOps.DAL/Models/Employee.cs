namespace RestaurantOps.DAL.Models
{
    public class Employee : BaseModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Shift> Shifts { get; set; } = new List<Shift>();
        public List<InventoryOrder> InventoryOrders { get; set; } = new List<InventoryOrder>();
    }
}
