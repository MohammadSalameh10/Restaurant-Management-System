using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CustomerRequest request);
        Task<bool> UpdateAsync(int id, CustomerRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
