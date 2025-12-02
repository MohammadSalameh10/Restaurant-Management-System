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

        public List<MenuItemIngredientResponse> GetAll()
        {
            var list = _menuItemIngredientRepository.GetAll();
            return list.Adapt<List<MenuItemIngredientResponse>>();
        }

        public MenuItemIngredientResponse GetById(int id)
        {
            var entity = _menuItemIngredientRepository.GetById(id);
            return entity?.Adapt<MenuItemIngredientResponse>();
        }

        public bool Create(MenuItemIngredientRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<MenuItemIngredient>();

            _menuItemIngredientRepository.Add(entity);
            _menuItemIngredientRepository.Save();
            return true;
        }

        public bool Update(int id, MenuItemIngredientRequest request)
        {
            var entity = _menuItemIngredientRepository.GetById(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

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
    }
}
