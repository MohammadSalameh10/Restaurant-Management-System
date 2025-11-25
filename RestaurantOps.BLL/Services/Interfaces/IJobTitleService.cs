using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IJobTitleService
    {
        List<JobTitleResponse> GetAll();
        JobTitleResponse GetById(int id);
        bool Create(JobTitleRequest request);
        bool Update(int id, JobTitleRequest request);
        bool Delete(int id);
    }
}
