using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleService(IJobTitleRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        public async Task<List<JobTitleResponse>> GetAllAsync()
        {
            var list = await _jobTitleRepository.GetAllAsync();
            return list.Adapt<List<JobTitleResponse>>();
        }

        public async Task<JobTitleResponse?> GetByIdAsync(int id)
        {
            var job = await _jobTitleRepository.GetByIdAsync(id);
            return job?.Adapt<JobTitleResponse>();
        }

        public async Task<bool> CreateAsync(JobTitleRequest request)
        {
            if (request == null)
                return false;

            var entity = request.Adapt<JobTitle>();

            await _jobTitleRepository.AddAsync(entity);
            await _jobTitleRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, JobTitleRequest request)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

            await _jobTitleRepository.UpdateAsync(entity);
            await _jobTitleRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _jobTitleRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _jobTitleRepository.DeleteAsync(entity);
            await _jobTitleRepository.SaveAsync();

            return true;
        }
    }
}
