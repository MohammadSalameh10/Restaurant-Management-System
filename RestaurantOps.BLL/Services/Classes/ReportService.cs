using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly IOrderRepository _orderRepository;

        public ReportService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public SalesReportResponse GetSalesReport()
        {
            var orders = _orderRepository.GetAllWithDetails();

            var today = DateTime.UtcNow.Date;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var todayOrders = orders.Where(o => o.Date.Date == today).ToList();
            var monthlyOrders = orders.Where(o => o.Date >= thisMonth).ToList();

            var todayRevenue = todayOrders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));
            var monthlyRevenue = monthlyOrders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));

            var statusCount = orders
                .GroupBy(o => o.OrderStatusEnum.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            var topItems = orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(i => i.MenuItem.ItemName)
                .Select(g => new TopMenuItemResponse
                {
                    ItemName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();

            return new SalesReportResponse
            {
                TodayRevenue = todayRevenue,
                MonthlyRevenue = monthlyRevenue,
                TodayOrders = todayOrders.Count,
                MonthlyOrders = monthlyOrders.Count,
                OrdersByStatus = statusCount,
                TopSellingItems = topItems
            };

        }
        public List<EmployeePerformanceResponse> GetEmployeePerformanceReport()
        {
            var orders = _orderRepository.GetAllWithDetails();

            var today = DateTime.UtcNow.Date;

            var grouped = orders
                .Where(o => o.Employee != null)
                .GroupBy(o => new { o.EmployeeId, o.Employee.Name });

            var list = new List<EmployeePerformanceResponse>();

            foreach (var g in grouped)
            {
                var totalOrders = g.Count();
                var totalRevenue = g.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));

                var todayOrders = g.Where(o => o.Date.Date == today).ToList();
                var todayOrdersCount = todayOrders.Count;
                var todayRevenue = todayOrders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));

                list.Add(new EmployeePerformanceResponse
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.Name,
                    TotalOrders = totalOrders,
                    TotalRevenue = totalRevenue,
                    TodayOrders = todayOrdersCount,
                    TodayRevenue = todayRevenue
                });
            }

            return list
                .OrderByDescending(e => e.TotalRevenue)
                .ToList();
        }
    }
}
