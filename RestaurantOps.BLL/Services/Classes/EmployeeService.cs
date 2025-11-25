using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<EmployeeResponse> GetAll()
        {
            var employees = _employeeRepository.GetAll();
            return employees.Select(MapToResponse).ToList();
        }

        public EmployeeResponse GetById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null) return null;

            return MapToResponse(employee);
        }

        public bool Create(EmployeeRequest request)
        {
            if (request == null) return false;

            var entity = new Employee
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                JobTitleId = request.JobTitleId
            };

            _employeeRepository.Add(entity);
            _employeeRepository.Save();
            return true;
        }

        public bool Update(int id, EmployeeRequest request)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null) return false;

            employee.Name = request.Name;
            employee.DateOfBirth = request.DateOfBirth;
            employee.JobTitleId = request.JobTitleId;

            _employeeRepository.Update(employee);
            _employeeRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null) return false;

            _employeeRepository.Delete(employee);
            _employeeRepository.Save();
            return true;
        }

        private EmployeeResponse MapToResponse(Employee e)
        {
            return new EmployeeResponse
            {
                Id = e.Id,
                Name = e.Name,
                DateOfBirth = e.DateOfBirth,
                JobTitleId = e.JobTitleId,
                JobTitleName = e.JobTitle?.Name,
                JobTitlePayRate = e.JobTitle?.PayRate ?? 0
            };
        }
    }
}
