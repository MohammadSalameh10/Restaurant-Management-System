using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IMenuItemIngredientRepository
    {
        Task<List<MenuItemIngredient>> GetAllAsync();
        Task<MenuItemIngredient?> GetByIdAsync(int id);
        Task<List<MenuItemIngredient>> GetByMenuItemIdAsync(int menuItemId);
        Task<List<MenuItemIngredient>> GetByMenuItemIdsAsync(List<int> menuItemIds);
        Task AddAsync(MenuItemIngredient entity);
        Task UpdateAsync(MenuItemIngredient entity);
        Task DeleteAsync(MenuItemIngredient entity);
        Task SaveAsync();
    }
}
