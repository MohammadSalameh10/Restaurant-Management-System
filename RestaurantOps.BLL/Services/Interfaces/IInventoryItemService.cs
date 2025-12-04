using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IInventoryItemService
    {
        Task<List<InventoryItemResponse>> GetAllAsync();
        Task<InventoryItemResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(InventoryItemRequest request);
        Task<bool> UpdateAsync(int id, InventoryItemRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<InventoryItemResponse>> GetLowStockAsync(decimal threshold);
    }
}
