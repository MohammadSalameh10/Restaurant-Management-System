using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAll();
        MenuItem GetById(int id);
        void Add(MenuItem menuItem);
        void Update(MenuItem menuItem);
        void Delete(MenuItem menuItem);
        void Save();
    }
}
