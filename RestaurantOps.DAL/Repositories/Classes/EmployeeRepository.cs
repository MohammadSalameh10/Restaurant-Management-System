using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees
                .Include(e => e.JobTitle)
                .ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees
                .Include(e => e.JobTitle)
                .FirstOrDefault(e => e.Id == id);
        }

        public void Add(Employee employee)
        {
            employee.CreatedAt = DateTime.UtcNow;
            employee.status = Status.Active;
            _context.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Employee GetByUserId(string userId)
        {
            return _context.Employees.FirstOrDefault(e => e.UserId == userId);
        }

    }
}
