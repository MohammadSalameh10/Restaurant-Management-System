using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IOrderTypeRepository
    {
        List<OrderType> GetAll();
        OrderType GetById(int id);
        void Add(OrderType type);
        void Update(OrderType type);
        void Delete(OrderType type);
        void Save();
    }
}
