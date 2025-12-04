using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<List<MenuItemResponse>> GetAllAsync();
        Task<MenuItemResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(MenuItemRequest request);
        Task<bool> UpdateAsync(int id, MenuItemRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
