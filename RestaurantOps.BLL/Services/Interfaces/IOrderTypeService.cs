using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IOrderTypeService
    {
        Task<List<OrderTypeResponse>> GetAllAsync();
        Task<OrderTypeResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(OrderTypeRequest request);
        Task<bool> UpdateAsync(int id, OrderTypeRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
