using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IMenuItemIngredientService
    {
        Task<List<MenuItemIngredientResponse>> GetAllAsync();
        Task<MenuItemIngredientResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(MenuItemIngredientRequest request);
        Task<bool> UpdateAsync(int id, MenuItemIngredientRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
