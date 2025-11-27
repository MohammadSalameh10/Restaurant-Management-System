using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class MenuItemIngredientRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemIngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MenuItemIngredient> GetAll()
        {
            return _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .ToList();
        }

        public MenuItemIngredient GetById(int id)
        {
            return _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .FirstOrDefault(m => m.Id == id);
        }

        public List<MenuItemIngredient> GetByMenuItemId(int menuItemId)
        {
            return _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .Where(m => m.MenuItemId == menuItemId)
                .ToList();
        }

        public List<MenuItemIngredient> GetByMenuItemIds(List<int> menuItemIds)
        {
            return _context.MenuItemIngredients
                .Include(m => m.MenuItem)
                .Include(m => m.InventoryItem)
                .Where(m => menuItemIds.Contains(m.MenuItemId))
                .ToList();
        }

        public void Add(MenuItemIngredient entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.status = Status.Active;
            _context.MenuItemIngredients.Add(entity);
        }

        public void Update(MenuItemIngredient entity)
        {
            _context.MenuItemIngredients.Update(entity);
        }

        public void Delete(MenuItemIngredient entity)
        {
            _context.MenuItemIngredients.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
