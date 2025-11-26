using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderStatusService
    {
        List<OrderStatusResponse> GetAll();
        OrderStatusResponse GetById(int id);
        bool Create(OrderStatusRequest request);
        bool Update(int id, OrderStatusRequest request);
        bool Delete(int id);
    }
}
