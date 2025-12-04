using Mapster;
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

        public async Task<List<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.Adapt<List<EmployeeResponse>>();
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee?.Adapt<EmployeeResponse>();
        }

        public async Task<bool> CreateAsync(EmployeeRequest request)
        {
            if (request == null)
                return false;

            var entity = new Employee
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                JobTitleId = request.JobTitleId
            };

            await _employeeRepository.AddAsync(entity);
            await _employeeRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, EmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            employee.Name = request.Name;
            employee.DateOfBirth = request.DateOfBirth;
            employee.JobTitleId = request.JobTitleId;

            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return false;

            await _employeeRepository.DeleteAsync(employee);
            await _employeeRepository.SaveAsync();

            return true;
        }
    }
}
