using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<List<MenuItemResponse>> GetAllAsync()
        {
            var items = await _menuItemRepository.GetAllAsync();
            return items.Adapt<List<MenuItemResponse>>();
        }

        public async Task<MenuItemResponse?> GetByIdAsync(int id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            return item?.Adapt<MenuItemResponse>();
        }

        public async Task<bool> CreateAsync(MenuItemRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<MenuItem>();

            await _menuItemRepository.AddAsync(entity);
            await _menuItemRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, MenuItemRequest request)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null)
                return false;

            request.Adapt(item);

            await _menuItemRepository.UpdateAsync(item);
            await _menuItemRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null)
                return false;

            await _menuItemRepository.DeleteAsync(item);
            await _menuItemRepository.SaveAsync();

            return true;
        }
    }
}
