using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<LocationResponse>> GetAllAsync();
        Task<LocationResponse?> GetByIdAsync(int id);
        Task<int> CreateAsync(LocationRequest request);
        Task<bool> UpdateAsync(int id, LocationRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
