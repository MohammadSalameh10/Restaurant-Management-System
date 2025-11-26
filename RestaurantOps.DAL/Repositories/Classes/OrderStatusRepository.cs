using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderStatus> GetAll()
        {
            return _context.OrderStatuses.ToList();
        }

        public OrderStatus GetById(int id)
        {
            return _context.OrderStatuses.FirstOrDefault(s => s.Id == id);
        }

        public void Add(OrderStatus status)
        {
            status.CreatedAt = DateTime.UtcNow;
            status.status = Status.Active;
            _context.OrderStatuses.Add(status);
        }

        public void Update(OrderStatus status)
        {
            _context.OrderStatuses.Update(status);
        }

        public void Delete(OrderStatus status)
        {
            _context.OrderStatuses.Remove(status);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
