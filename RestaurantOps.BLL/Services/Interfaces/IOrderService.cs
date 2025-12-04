using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(int id);
        Task<int> CreateOrderAsync(OrderCreateRequest request);
        Task<bool> ChangeStatusAsync(int id, OrderStatus newStatus);
        Task<bool> DeleteAsync(int id);
        Task<List<OrderResponse>> GetCustomerOrdersAsync(string userId);
        Task<bool> CancelOrderForCustomerAsync(int orderId, string userId);
        Task<List<OrderResponse>> GetOrdersForEmployeeAsync(int employeeId);
        Task<bool> AssignOrderToEmployeeAsync(int orderId, int employeeId);
    }
}
