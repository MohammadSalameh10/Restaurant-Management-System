using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderResponse> GetAll();
        OrderResponse GetById(int id);
        int CreateOrder(OrderCreateRequest request);
        bool ChangeStatus(int id, OrderStatus newStatus);
        bool Delete(int id);
        List<OrderResponse> GetCustomerOrders(string userId);
        bool CancelOrderForCustomer(int orderId, string userId);
        List<OrderResponse> GetOrdersForEmployee(int employeeId);

    }
}
