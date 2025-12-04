using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IJobTitleService
    {
        Task<List<JobTitleResponse>> GetAllAsync();
        Task<JobTitleResponse?> GetByIdAsync(int id);
        Task<bool> CreateAsync(JobTitleRequest request);
        Task<bool> UpdateAsync(int id, JobTitleRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
