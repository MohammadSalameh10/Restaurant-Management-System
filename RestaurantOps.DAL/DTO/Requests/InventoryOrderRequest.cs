namespace RestaurantOps.DAL.DTO.Requests
{
    public class InventoryOrderRequest
    {
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public List<InventoryOrderItemRequest> Items { get; set; } = new();
    }
}
