using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.DTO.Responses
{
    public class AdminDashboardResponse
    {
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalEmployees { get; set; }
    }
}
