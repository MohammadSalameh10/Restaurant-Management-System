using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Supplier> GetAll()
        {
            return _context.Suppliers
                .Include(s => s.Location)
                .ToList();
        }

        public Supplier GetById(int id)
        {
            return _context.Suppliers
                .Include(s => s.Location)
                .FirstOrDefault(s => s.Id == id);
        }

        public void Add(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.status = Status.Active;
            _context.Suppliers.Add(supplier);
        }

        public void Update(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
        }

        public void Delete(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
