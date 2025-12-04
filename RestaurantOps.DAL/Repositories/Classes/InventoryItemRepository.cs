using Microsoft.EntityFrameworkCore;
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

        public async Task<List<InventoryItem>> GetAllAsync()
        {
            return await _context.InventoryItems
                .Include(i => i.Supplier)
                .ToListAsync();
        }

        public async Task<InventoryItem?> GetByIdAsync(int id)
        {
            return await _context.InventoryItems
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(InventoryItem item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.Status = Status.Active;
            await _context.InventoryItems.AddAsync(item);
        }

        public Task UpdateAsync(InventoryItem item)
        {
            _context.InventoryItems.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(InventoryItem item)
        {
            _context.InventoryItems.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<InventoryItem>> GetLowStockItemsAsync(decimal threshold)
        {
            return await _context.InventoryItems
                .Include(i => i.Supplier)
                .Where(i => i.Stock <= threshold)
                .ToListAsync();
        }
    }
}
