using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<List<MenuItem>> GetByIdsWithIngredientsAsync(List<int> ids)
        {
            return await _context.MenuItems
                .Include(m => m.Ingredients)
                    .ThenInclude(i => i.InventoryItem)
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(MenuItem menuItem)
        {
            menuItem.CreatedAt = DateTime.UtcNow;
            menuItem.Status = Status.Active;
            await _context.MenuItems.AddAsync(menuItem);
        }

        public Task UpdateAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MenuItem menuItem)
        {
            _context.MenuItems.Remove(menuItem);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
