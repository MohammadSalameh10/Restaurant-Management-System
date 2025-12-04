using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IJobTitleRepository
    {
        Task<List<JobTitle>> GetAllAsync();
        Task<JobTitle?> GetByIdAsync(int id);
        Task AddAsync(JobTitle jobTitle);
        Task UpdateAsync(JobTitle jobTitle);
        Task DeleteAsync(JobTitle jobTitle);
        Task SaveAsync();
    }
}
