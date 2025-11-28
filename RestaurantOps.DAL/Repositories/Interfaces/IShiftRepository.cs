using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IShiftRepository
    {
        List<Shift> GetAll();
        Shift GetById(int id);
        void Add(Shift shift);
        void Update(Shift shift);
        void Delete(Shift shift);
        void Save();
        List<Shift> GetEmployeeShifts(int employeeId, DateTime? from = null, DateTime? to = null);
    }
}
