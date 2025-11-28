using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IShiftService
    {
        List<ShiftResponse> GetAll();
        ShiftResponse GetById(int id);
        bool Create(ShiftRequest request);
        bool Update(int id, ShiftRequest request);
        bool Delete(int id);
        List<ShiftResponse> GetEmployeeShifts(int employeeId, DateTime? from = null, DateTime? to = null);
        bool EmployeeCheckIn(string userId);
        bool EmployeeCheckOut(string userId);
    }
}
