using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ISupplierService
    {
        List<SupplierResponse> GetAll();
        SupplierResponse GetById(int id);
        int Create(SupplierRequest request);
        bool Update(int id, SupplierRequest request);
        bool Delete(int id);
    }
}
