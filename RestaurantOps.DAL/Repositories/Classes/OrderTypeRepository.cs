using Microsoft.EntityFrameworkCore;
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

        public async Task<List<OrderType>> GetAllAsync()
        {
            return await _context.OrderTypes.ToListAsync();
        }

        public async Task<OrderType?> GetByIdAsync(int id)
        {
            return await _context.OrderTypes.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(OrderType type)
        {
            type.CreatedAt = DateTime.UtcNow;
            type.Status = Status.Active;
            await _context.OrderTypes.AddAsync(type);
        }

        public Task UpdateAsync(OrderType type)
        {
            _context.OrderTypes.Update(type);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(OrderType type)
        {
            _context.OrderTypes.Remove(type);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
