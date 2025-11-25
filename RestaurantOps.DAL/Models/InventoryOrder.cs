namespace RestaurantOps.DAL.Models
{
    public class InventoryOrder : BaseModel
    {
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public List<InventoryOrderItem> Items { get; set; } = new List<InventoryOrderItem>();
    }
}
