using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IInventoryOrderService
    {
        List<InventoryOrderResponse> GetAll();
        InventoryOrderResponse GetById(int id);
        int Create(InventoryOrderRequest request);
        bool Delete(int id);
    }
}
