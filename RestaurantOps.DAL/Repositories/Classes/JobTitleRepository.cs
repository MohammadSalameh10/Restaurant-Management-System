using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.DAL.Repositories.Classes
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly ApplicationDbContext _context;

        public JobTitleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<JobTitle> GetAll()
        {
            return _context.JobTitles.ToList();
        }

        public JobTitle GetById(int id)
        {
            return _context.JobTitles.FirstOrDefault(j => j.Id == id);
        }

        public void Add(JobTitle jobTitle)
        {
            jobTitle.CreatedAt = DateTime.UtcNow;
            jobTitle.status = Status.Active;
            _context.JobTitles.Add(jobTitle);
        }

        public void Update(JobTitle jobTitle)
        {
            _context.JobTitles.Update(jobTitle);
        }

        public void Delete(JobTitle jobTitle)
        {
            _context.JobTitles.Remove(jobTitle);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
