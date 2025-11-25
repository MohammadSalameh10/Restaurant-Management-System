using RestaurantOps.BLL.Services.Classes;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.Repositories.Classes;
using RestaurantOps.DAL.Repositories.Interfaces;

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
            services.AddScoped<IInventoryItemRepository, DAL.Repositories.Classes.InventoryItemRepository>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
        }
    }
}
