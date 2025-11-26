namespace RestaurantOps.DAL.DTO.Responses
{
    public class InventoryOrderResponse
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public decimal TotalCost { get; set; } 

        public List<InventoryOrderItemResponse> Items { get; set; }
    }
}
