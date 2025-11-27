using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAll();
        public List<MenuItem> GetByIdsWithIngredients(List<int> id);
        MenuItem GetById(int id);
        void Add(MenuItem menuItem);
        void Update(MenuItem menuItem);
        void Delete(MenuItem menuItem);
        void Save();
    }
}
