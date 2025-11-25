using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public List<ShiftResponse> GetAll()
        {
            var shifts = _shiftRepository.GetAll();
            return shifts.Select(MapToResponse).ToList();
        }

        public ShiftResponse GetById(int id)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift == null) return null;

            return MapToResponse(shift);
        }

        public bool Create(ShiftRequest request)
        {
            if (request == null) return false;

            var entity = new Shift
            {
                EmployeeId = request.EmployeeId,
                ExpectedCheckIn = request.ExpectedCheckIn,
                ExpectedCheckOut = request.ExpectedCheckOut,
                CheckIn = (DateTime)request.CheckIn,
                CheckOut = request.CheckOut,
                HasWorked = request.HasWorked
            };

            _shiftRepository.Add(entity);
            _shiftRepository.Save();
            return true;
        }

        public bool Update(int id, ShiftRequest request)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift == null) return false;

            shift.EmployeeId = request.EmployeeId;
            shift.ExpectedCheckIn = request.ExpectedCheckIn;
            shift.ExpectedCheckOut = request.ExpectedCheckOut;
            shift.CheckIn = (DateTime)request.CheckIn;
            shift.CheckOut = request.CheckOut;
            shift.HasWorked = request.HasWorked;

            _shiftRepository.Update(shift);
            _shiftRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var shift = _shiftRepository.GetById(id);
            if (shift == null) return false;

            _shiftRepository.Delete(shift);
            _shiftRepository.Save();
            return true;
        }

        public List<ShiftResponse> GetEmployeeShifts(int employeeId, DateTime? from = null, DateTime? to = null)
        {
            var shifts = _shiftRepository.GetEmployeeShifts(employeeId, from, to);
            return shifts.Select(MapToResponse).ToList();
        }

        private ShiftResponse MapToResponse(Shift s)
        {
            return new ShiftResponse
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee?.Name,
                ExpectedCheckIn = s.ExpectedCheckIn,
                ExpectedCheckOut = s.ExpectedCheckOut,
                CheckIn = s.CheckIn,
                CheckOut = s.CheckOut,
                HasWorked = s.HasWorked
            };
        }
    }
}
