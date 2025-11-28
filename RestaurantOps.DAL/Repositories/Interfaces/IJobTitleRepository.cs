using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IJobTitleRepository
    {
        List<JobTitle> GetAll();
        JobTitle GetById(int id);
        void Add(JobTitle jobTitle);
        void Update(JobTitle jobTitle);
        void Delete(JobTitle jobTitle);
        void Save();
    }
}
