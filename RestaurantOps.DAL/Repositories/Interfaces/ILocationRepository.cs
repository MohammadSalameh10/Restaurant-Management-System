using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        List<Location> GetAll();
        Location GetById(int id);
        void Add(Location location);
        void Update(Location location);
        void Delete(Location location);
        void Save();
    }
}
