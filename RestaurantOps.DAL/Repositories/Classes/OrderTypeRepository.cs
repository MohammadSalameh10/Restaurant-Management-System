using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class OrderTypeRepository : IOrderTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderType> GetAll()
        {
            return _context.OrderTypes.ToList();
        }

        public OrderType GetById(int id)
        {
            return _context.OrderTypes.FirstOrDefault(t => t.Id == id);
        }

        public void Add(OrderType type)
        {
            type.CreatedAt = DateTime.UtcNow;
            type.status = Status.Active;
            _context.OrderTypes.Add(type);
        }

        public void Update(OrderType type)
        {
            _context.OrderTypes.Update(type);
        }

        public void Delete(OrderType type)
        {
            _context.OrderTypes.Remove(type);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
