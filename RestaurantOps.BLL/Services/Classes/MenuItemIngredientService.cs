using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<MenuItemIngredientResponse> GetAll()
        {
            var list = _menuItemIngredientRepository.GetAll();
            return list.Select(MapToResponse).ToList();
        }

        public MenuItemIngredientResponse GetById(int id)
        {
            var entity = _menuItemIngredientRepository.GetById(id);
            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        public bool Create(MenuItemIngredientRequest request)
        {
            if (request == null)
                return false;

            var entity = new MenuItemIngredient
            {
                Quantity = request.Quantity,
                MenuItemId = request.MenuItemId,
                InventoryItemId = request.InventoryItemId
            };

            _menuItemIngredientRepository.Add(entity);
            _menuItemIngredientRepository.Save();
            return true;
        }

        public bool Update(int id, MenuItemIngredientRequest request)
        {
            var entity = _menuItemIngredientRepository.GetById(id);
            if (entity == null)
                return false;

            entity.Quantity = request.Quantity;
            entity.MenuItemId = request.MenuItemId;
            entity.InventoryItemId = request.InventoryItemId;

            _menuItemIngredientRepository.Update(entity);
            _menuItemIngredientRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _menuItemIngredientRepository.GetById(id);
            if (entity == null)
                return false;

            _menuItemIngredientRepository.Delete(entity);
            _menuItemIngredientRepository.Save();
            return true;
        }

        private MenuItemIngredientResponse MapToResponse(MenuItemIngredient m)
        {
            return new MenuItemIngredientResponse
            {
                Id = m.Id,
                Quantity = m.Quantity,
                MenuItemId = m.MenuItemId,
                MenuItemName = m.MenuItem?.ItemName,
                InventoryItemId = m.InventoryItemId,
                InventoryItemName = m.InventoryItem?.Name
            };
        }
    }
}
