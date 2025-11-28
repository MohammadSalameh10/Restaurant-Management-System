using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IMenuItemIngredientRepository
    {
        List<MenuItemIngredient> GetAll();
        MenuItemIngredient GetById(int id);
        List<MenuItemIngredient> GetByMenuItemId(int menuItemId);
        List<MenuItemIngredient> GetByMenuItemIds(List<int> menuItemIds);
        void Add(MenuItemIngredient entity);
        void Update(MenuItemIngredient entity);
        void Delete(MenuItemIngredient entity);
        void Save();
    }
}
