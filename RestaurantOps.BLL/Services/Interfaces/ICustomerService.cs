using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerResponse> GetAll();
        CustomerResponse GetById(int id);
        bool Create(CustomerRequest request);
        bool Update(int id, CustomerRequest request);
        bool Delete(int id);
    }
}
