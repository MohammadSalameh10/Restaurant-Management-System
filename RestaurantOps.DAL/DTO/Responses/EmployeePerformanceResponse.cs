namespace RestaurantOps.DAL.DTO.Responses
{
    public class EmployeePerformanceResponse
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TodayOrders { get; set; }
        public decimal TodayRevenue { get; set; }
    }
}
