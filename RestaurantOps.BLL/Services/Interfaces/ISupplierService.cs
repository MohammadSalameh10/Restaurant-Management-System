using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierResponse>> GetAllAsync();
        Task<SupplierResponse?> GetByIdAsync(int id);
        Task<int> CreateAsync(SupplierRequest request);
        Task<bool> UpdateAsync(int id, SupplierRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
