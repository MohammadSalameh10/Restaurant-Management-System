using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<LocationResponse> GetAll()
        {
            var list = _locationRepository.GetAll();
            return list.Adapt<List<LocationResponse>>();
        }

        public LocationResponse GetById(int id)
        {
            var location = _locationRepository.GetById(id);
            if (location == null)
                return null;

            return location?.Adapt<LocationResponse>();
        }

        public int Create(LocationRequest request)
        {
            if (request == null)
                return 0;

            var entity = request.Adapt<Location>();

            entity.CreatedAt = DateTime.UtcNow;
            entity.status = Status.Active;

            _locationRepository.Add(entity);
            _locationRepository.Save();

            return entity.Id;
        }

        public bool Update(int id, LocationRequest request)
        {
            var entity = _locationRepository.GetById(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

            _locationRepository.Update(entity);
            _locationRepository.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var entity = _locationRepository.GetById(id);
            if (entity == null)
                return false;

            _locationRepository.Delete(entity);
            _locationRepository.Save();
            return true;
        }
    }
}
