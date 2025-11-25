using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeResponse> GetAll();
        EmployeeResponse GetById(int id);
        bool Create(EmployeeRequest request);
        bool Update(int id, EmployeeRequest request);
        bool Delete(int id);
    }
}
