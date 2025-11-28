using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class DashboardService : IDashboardService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public DashboardService(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        public AdminDashboardResponse GetSummary()
        {
            var orders = _orderRepository.GetAllWithDetails();
            var payments = _paymentRepository.GetAll();
            var totalOrders = orders.Count;
            var completedOrders = orders.Count(o => o.OrderStatusEnum == OrderStatus.Completed);
            var pendingOrders = orders.Count(o => o.OrderStatusEnum == OrderStatus.Pending);
            var totalRevenue = payments.Sum(p => p.Amount);
            var today = DateTime.UtcNow.Date;
            var todayRevenue = payments
                .Where(p => p.PaidAt.Date == today)
                .Sum(p => p.Amount);
            var totalCustomers = _customerRepository.GetAll().Count;
            var totalEmployees = _employeeRepository.GetAll().Count;

            return new AdminDashboardResponse
            {
                TotalOrders = totalOrders,
                CompletedOrders = completedOrders,
                PendingOrders = pendingOrders,
                TotalRevenue = totalRevenue,
                TodayRevenue = todayRevenue,
                TotalCustomers = totalCustomers,
                TotalEmployees = totalEmployees
            };
        }
    }
}
