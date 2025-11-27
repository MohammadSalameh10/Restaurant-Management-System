using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IMenuItemIngredientService
    {
        List<MenuItemIngredientResponse> GetAll();
        MenuItemIngredientResponse GetById(int id);
        bool Create(MenuItemIngredientRequest request);
        bool Update(int id, MenuItemIngredientRequest request);
        bool Delete(int id);
    }
}
