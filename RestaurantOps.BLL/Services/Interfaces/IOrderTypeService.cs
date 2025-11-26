using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderTypeService
    {
        List<OrderTypeResponse> GetAll();
        OrderTypeResponse GetById(int id);
        bool Create(OrderTypeRequest request);
        bool Update(int id, OrderTypeRequest request);
        bool Delete(int id);
    }
}
