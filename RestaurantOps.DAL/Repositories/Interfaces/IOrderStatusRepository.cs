using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IOrderStatusRepository
    {
        List<OrderStatus> GetAll();
        OrderStatus GetById(int id);
        void Add(OrderStatus status);
        void Update(OrderStatus status);
        void Delete(OrderStatus status);
        void Save();
    }
}
