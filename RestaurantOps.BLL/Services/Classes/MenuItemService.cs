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

        public List<MenuItemResponse> GetAll()
        {
            var items = _menuItemRepository.GetAll();

            return items.Adapt<List<MenuItemResponse>>();
        }

        public MenuItemResponse GetById(int id)
        {
            var item = _menuItemRepository.GetById(id);
            return item?.Adapt<MenuItemResponse>();
        }

        public bool Create(MenuItemRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<MenuItem>();

            _menuItemRepository.Add(entity);
            _menuItemRepository.Save();
            return true;
        }

        public bool Update(int id, MenuItemRequest request)
        {
            var item = _menuItemRepository.GetById(id);
            if (item == null)
                return false;

            request.Adapt(item);

            _menuItemRepository.Update(item);
            _menuItemRepository.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var item = _menuItemRepository.GetById(id);
            if (item == null)
                return false;

            _menuItemRepository.Delete(item);
            _menuItemRepository.Save();

            return true;
        }
    }
}
