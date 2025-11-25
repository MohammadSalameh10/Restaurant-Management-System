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

        public List<MenuItem> GetAll()
        {
            return _context.MenuItems.ToList();
        }

        public MenuItem GetById(int id)
        {
            return _context.MenuItems.FirstOrDefault(m => m.Id == id);
        }

        public void Add(MenuItem menuItem)
        {
            menuItem.CreatedAt = DateTime.UtcNow;
            menuItem.status = Status.Active;

            _context.MenuItems.Add(menuItem);
        }

        public void Update(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
        }

        public void Delete(MenuItem menuItem)
        {
            _context.MenuItems.Remove(menuItem);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
