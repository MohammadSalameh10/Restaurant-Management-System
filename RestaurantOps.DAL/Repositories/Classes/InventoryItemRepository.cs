using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<InventoryItem> GetAll()
        {
            return _context.InventoryItems.ToList();
        }

        public InventoryItem GetById(int id)
        {
            return _context.InventoryItems.FirstOrDefault(i => i.Id == id);
        }

        public void Add(InventoryItem item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.status = Status.Active;
            _context.InventoryItems.Add(item);
        }

        public void Update(InventoryItem item)
        {
            _context.InventoryItems.Update(item);
        }

        public void Delete(InventoryItem item)
        {
            _context.InventoryItems.Remove(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public List<InventoryItem> GetLowStockItems(decimal threshold)
        {
            return _context.InventoryItems
                .Where(i => i.Stock <= threshold)
                .ToList();
        }
    }
}
