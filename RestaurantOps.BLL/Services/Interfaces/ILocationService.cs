using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface ILocationService
    {
        List<LocationResponse> GetAll();
        LocationResponse GetById(int id);
        int Create(LocationRequest request);
        bool Update(int id, LocationRequest request);
        bool Delete(int id);
    }
}
