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
            return list.Select(j => new JobTitleResponse
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                PayRate = j.PayRate
            }).ToList();
        }

        public JobTitleResponse GetById(int id)
        {
            var j = _jobTitleRepository.GetById(id);
            if (j == null) return null;

            return new JobTitleResponse
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                PayRate = j.PayRate
            };
        }

        public bool Create(JobTitleRequest request)
        {
            if (request == null) return false;

            var entity = new JobTitle
            {
                Name = request.Name,
                Description = request.Description,
                PayRate = request.PayRate
            };

            _jobTitleRepository.Add(entity);
            _jobTitleRepository.Save();
            return true;
        }

        public bool Update(int id, JobTitleRequest request)
        {
            var entity = _jobTitleRepository.GetById(id);
            if (entity == null) return false;

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.PayRate = request.PayRate;

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
