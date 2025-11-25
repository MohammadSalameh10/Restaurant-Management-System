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

            return items
                .Select(m => new MenuItemResponse
                {
                    Id = m.Id,
                    ItemName = m.ItemName,
                    Description = m.Description,
                    IsAvailable = m.IsAvailable
                })
                .ToList();
        }

        public MenuItemResponse GetById(int id)
        {
            var item = _menuItemRepository.GetById(id);
            if (item == null)
                return null;

            return new MenuItemResponse
            {
                Id = item.Id,
                ItemName = item.ItemName,
                Description = item.Description,
                IsAvailable = item.IsAvailable
            };
        }

        public bool Create(MenuItemRequest request)
        {
            if (request == null)
                return false;

            var entity = new MenuItem
            {
                ItemName = request.ItemName,
                Description = request.Description,
                IsAvailable = request.IsAvailable,
            };

            _menuItemRepository.Add(entity);
            _menuItemRepository.Save();
            return true;
        }

        public bool Update(int id, MenuItemRequest request)
        {
            var item = _menuItemRepository.GetById(id);
            if (item == null)
                return false;

            item.ItemName = request.ItemName;
            item.Description = request.Description;
            item.IsAvailable = request.IsAvailable;

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
