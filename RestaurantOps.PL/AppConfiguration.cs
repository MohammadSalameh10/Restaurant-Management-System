using Microsoft.AspNetCore.Identity.UI.Services;
using RestaurantOps.BLL.Services.Classes;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.Repositories.Classes;
using RestaurantOps.DAL.Repositories.Interfaces;
using RestaurantOps.DAL.Utils;
using RestaurantOps.PL.Utils;

namespace RestaurantOps.PL
{
    public static class AppConfiguration
    {
        public static void AddConfig(this IServiceCollection services)
        {
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IJobTitleRepository, JobTitleRepository>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IInventoryOrderRepository, InventoryOrderRepository>();
            services.AddScoped<IInventoryOrderService, InventoryOrderService>();
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();
            services.AddScoped<IOrderTypeRepository, OrderTypeRepository>();
            services.AddScoped<IOrderTypeService, OrderTypeService>();
            services.AddScoped<ISeedData, SeedData>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailSender, EmailSetting>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
