using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Add(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.status = Status.Active;

            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Order GetOrderWithDetails(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetAllWithDetails()
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ToList();
        }
    }
}
