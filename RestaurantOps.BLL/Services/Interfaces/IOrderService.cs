using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderResponse> GetAll();
        OrderResponse GetById(int id);
        int CreateOrder(OrderCreateRequest request);
        bool Delete(int id);
    }
}
