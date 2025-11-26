using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class InventoryOrderRepository : IInventoryOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<InventoryOrder> GetAll()
        {
            return _context.InventoryOrders
                .Include(o => o.Employee)
                .Include(o => o.Items)
                .ThenInclude(i => i.InventoryItem)
                .ToList();
        }

        public InventoryOrder GetById(int id)
        {
            return _context.InventoryOrders
                .Include(o => o.Employee)
                .Include(o => o.Items)
                    .ThenInclude(i => i.InventoryItem)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Add(InventoryOrder order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.status = Status.Active;
            _context.InventoryOrders.Add(order);
        }

        public void Update(InventoryOrder order)
        {
            _context.InventoryOrders.Update(order);
        }

        public void Delete(InventoryOrder order)
        {
            _context.InventoryOrders.Remove(order);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
