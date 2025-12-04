using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IOrderTypeRepository
    {
        Task<List<OrderType>> GetAllAsync();
        Task<OrderType?> GetByIdAsync(int id);
        Task AddAsync(OrderType type);
        Task UpdateAsync(OrderType type);
        Task DeleteAsync(OrderType type);
        Task SaveAsync();
    }
}
