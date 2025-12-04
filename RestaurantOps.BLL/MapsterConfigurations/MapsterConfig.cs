using Mapster;
using Microsoft.Extensions.DependencyInjection;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister(this IServiceCollection services)
        {
            TypeAdapterConfig<MenuItemIngredient, MenuItemIngredientResponse>
                .NewConfig()
                .Map(d => d.MenuItemName, s => s.MenuItem.ItemName);

            TypeAdapterConfig<OrderItem, OrderItemResponse>
                .NewConfig()
                .Map(d => d.MenuItemId, s => s.MenuItemId)
                .Map(d => d.MenuItemName, s => s.MenuItem.ItemName)
                .Map(d => d.Quantity, s => s.Quantity)
                .Map(d => d.Price, s => s.Price)
                .Map(d => d.Total, s => s.Quantity * s.Price);

            TypeAdapterConfig<Order, OrderResponse>
                .NewConfig()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Date, s => s.Date)
                .Map(d => d.Customer, s => s.Customer != null ? s.Customer.Name : null)
                .Map(d => d.Employee, s => s.Employee != null ? s.Employee.Name : null)
                .Map(d => d.OrderType, s => s.OrderType != null ? s.OrderType.Name : null)
                .Map(d => d.Status, s => s.OrderStatusEnum.ToString())
                .Map(d => d.Items, s => s.OrderItems)
                .Map(d => d.TotalAmount,
                     s => s.OrderItems.Sum(i => i.Quantity * i.Price));
        }
    }
}
