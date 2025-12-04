using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class MenuItemIngredientRepository : IMenuItemIngredientRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemIngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItemIngredient>> GetAllAsync()
        {
            return await _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .ToListAsync();
        }

        public async Task<MenuItemIngredient?> GetByIdAsync(int id)
        {
            return await _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<MenuItemIngredient>> GetByMenuItemIdAsync(int menuItemId)
        {
            return await _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .Where(m => m.MenuItemId == menuItemId)
                .ToListAsync();
        }

        public async Task<List<MenuItemIngredient>> GetByMenuItemIdsAsync(List<int> menuItemIds)
        {
            return await _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .Where(m => menuItemIds.Contains(m.MenuItemId))
                .ToListAsync();
        }

        public async Task AddAsync(MenuItemIngredient entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = Status.Active;
            await _context.MenuItemIngredients.AddAsync(entity);
        }

        public Task UpdateAsync(MenuItemIngredient entity)
        {
            _context.MenuItemIngredients.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MenuItemIngredient entity)
        {
            _context.MenuItemIngredients.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
