using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Location> GetAll()
        {
            return _context.Locations.ToList();
        }

        public Location GetById(int id)
        {
            return _context.Locations.FirstOrDefault(l => l.Id == id);
        }

        public void Add(Location location)
        {
            location.CreatedAt = DateTime.UtcNow;
            location.status = Status.Active;
            _context.Locations.Add(location);
        }

        public void Update(Location location)
        {
            _context.Locations.Update(location);
        }

        public void Delete(Location location)
        {
            _context.Locations.Remove(location);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
