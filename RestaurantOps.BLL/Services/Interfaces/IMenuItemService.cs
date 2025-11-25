using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IMenuItemService
    {
        List<MenuItemResponse> GetAll();
        MenuItemResponse GetById(int id);
        bool Create(MenuItemRequest request);
        bool Update(int id, MenuItemRequest request);
        bool Delete(int id);
    }
}
