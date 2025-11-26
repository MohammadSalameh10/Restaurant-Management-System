using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        List<Supplier> GetAll();
        Supplier GetById(int id);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(Supplier supplier);
        void Save();
    }
}
