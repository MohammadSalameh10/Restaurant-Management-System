using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll()
        {
            return _context.Customers
                .Include(c => c.Location)
                .ToList();
        }

        public Customer GetById(int id)
        {
            return _context.Customers.Include(c => c.Location)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            customer.status = Status.Active;
            _context.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Customer GetByUserId(string userId)
        {
            return _context.Customers.FirstOrDefault(c => c.UserId == userId);
        }

    }
}
