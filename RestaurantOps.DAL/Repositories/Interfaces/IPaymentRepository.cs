using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();
        Payment GetById(int id);
        Payment GetByOrderId(int orderId);
        void Add(Payment payment);
        void Update(Payment payment);
        void Delete(Payment payment);
        void Save();
    }
}
