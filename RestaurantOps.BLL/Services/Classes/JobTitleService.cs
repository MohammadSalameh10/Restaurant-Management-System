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

        public List<JobTitleResponse> GetAll()
        {
            var list = _jobTitleRepository.GetAll();
            return list.Adapt<List<JobTitleResponse>>();
        }

        public JobTitleResponse GetById(int id)
        {
            var job = _jobTitleRepository.GetById(id);
            if (job == null) return null;

            return job?.Adapt<JobTitleResponse>();
        }

        public bool Create(JobTitleRequest request)
        {
            if (request == null) return false;

            var entity = request.Adapt<JobTitle>();

            _jobTitleRepository.Add(entity);
            _jobTitleRepository.Save();
            return true;
        }

        public bool Update(int id, JobTitleRequest request)
        {
            var entity = _jobTitleRepository.GetById(id);
            if (entity == null) return false;

            request.Adapt(entity);

            _jobTitleRepository.Update(entity);
            _jobTitleRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _jobTitleRepository.GetById(id);
            if (entity == null) return false;

            _jobTitleRepository.Delete(entity);
            _jobTitleRepository.Save();
            return true;
        }
    }
}
