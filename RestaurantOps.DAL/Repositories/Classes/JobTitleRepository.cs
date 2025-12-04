using Microsoft.EntityFrameworkCore;
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

        public async Task<List<JobTitle>> GetAllAsync()
        {
            return await _context.JobTitles.ToListAsync();
        }

        public async Task<JobTitle?> GetByIdAsync(int id)
        {
            return await _context.JobTitles
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task AddAsync(JobTitle jobTitle)
        {
            jobTitle.CreatedAt = DateTime.UtcNow;
            jobTitle.Status = Status.Active;
            await _context.JobTitles.AddAsync(jobTitle);
        }

        public Task UpdateAsync(JobTitle jobTitle)
        {
            _context.JobTitles.Update(jobTitle);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(JobTitle jobTitle)
        {
            _context.JobTitles.Remove(jobTitle);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
