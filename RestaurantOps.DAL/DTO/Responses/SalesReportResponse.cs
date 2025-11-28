namespace RestaurantOps.DAL.DTO.Responses
{
    public class SalesReportResponse
    {
        public decimal TodayRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }

        public int TodayOrders { get; set; }
        public int MonthlyOrders { get; set; }

        public Dictionary<string, int> OrdersByStatus { get; set; }

        public List<TopMenuItemResponse> TopSellingItems { get; set; }
    }
}
