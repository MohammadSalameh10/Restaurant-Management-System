using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IInventoryItemService
    {
        List<InventoryItemResponse> GetAll();
        InventoryItemResponse GetById(int id);
        bool Create(InventoryItemRequest request);
        bool Update(int id, InventoryItemRequest request);
        bool Delete(int id);
        List<InventoryItemResponse> GetLowStock(decimal threshold);
    }
}
