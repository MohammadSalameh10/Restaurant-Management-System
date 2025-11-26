using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetAll()
        {
            return _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.OrderStatus)
                .ToList();
        }

        public Payment GetById(int id)
        {
            return _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.OrderStatus)
                .FirstOrDefault(p => p.Id == id);
        }

        public Payment GetByOrderId(int orderId)
        {
            return _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.OrderStatus)
                .FirstOrDefault(p => p.OrderId == orderId);
        }

        public void Add(Payment payment)
        {
            payment.CreatedAt = DateTime.UtcNow;
            payment.status = Status.Active;
            _context.Payments.Add(payment);
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
        }

        public void Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
