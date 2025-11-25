using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Shift> GetAll()
        {
            return _context.Shifts
                .Include(s => s.Employee)
                .ToList();
        }

        public Shift GetById(int id)
        {
            return _context.Shifts
                .Include(s => s.Employee)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Add(Shift shift)
        {
            _context.Shifts.Add(shift);
        }

        public void Update(Shift shift)
        {
            _context.Shifts.Update(shift);
        }

        public void Delete(Shift shift)
        {
            _context.Shifts.Remove(shift);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public List<Shift> GetEmployeeShifts(int employeeId, DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.EmployeeId == employeeId);

            if (from.HasValue)
                query = query.Where(s => s.ExpectedCheckIn >= from.Value);

            if (to.HasValue)
                query = query.Where(s => s.ExpectedCheckOut <= to.Value);

            return query.ToList();
        }
    }
}
