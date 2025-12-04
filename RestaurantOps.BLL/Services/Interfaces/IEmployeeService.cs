using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponse>> GetAllAsync();
        Task<EmployeeResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(EmployeeRequest request);
        Task<bool> UpdateAsync(int id, EmployeeRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
