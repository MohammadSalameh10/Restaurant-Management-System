namespace RestaurantOps.DAL.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Preparing = 2,
        Completed = 3,
        Ready = 4,
        Delivered = 5,
        Canceled = 6
    }
    public class Order : BaseModel
    {
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public OrderStatus OrderStatusEnum { get; set; }
        public int OrderTypeId { get; set; }
        public OrderType OrderType { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
