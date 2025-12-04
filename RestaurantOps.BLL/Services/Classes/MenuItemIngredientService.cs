using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class MenuItemIngredientService : IMenuItemIngredientService
    {
        private readonly IMenuItemIngredientRepository _menuItemIngredientRepository;

        public MenuItemIngredientService(IMenuItemIngredientRepository menuItemIngredientRepository)
        {
            _menuItemIngredientRepository = menuItemIngredientRepository;
        }

        public async Task<List<MenuItemIngredientResponse>> GetAllAsync()
        {
            var list = await _menuItemIngredientRepository.GetAllAsync();
            return list.Adapt<List<MenuItemIngredientResponse>>();
        }

        public async Task<MenuItemIngredientResponse?> GetByIdAsync(int id)
        {
            var entity = await _menuItemIngredientRepository.GetByIdAsync(id);
            return entity?.Adapt<MenuItemIngredientResponse>();
        }

        public async Task<bool> CreateAsync(MenuItemIngredientRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<MenuItemIngredient>();

            await _menuItemIngredientRepository.AddAsync(entity);
            await _menuItemIngredientRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, MenuItemIngredientRequest request)
        {
            var entity = await _menuItemIngredientRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

            await _menuItemIngredientRepository.UpdateAsync(entity);
            await _menuItemIngredientRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _menuItemIngredientRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _menuItemIngredientRepository.DeleteAsync(entity);
            await _menuItemIngredientRepository.SaveAsync();

            return true;
        }
    }
}
