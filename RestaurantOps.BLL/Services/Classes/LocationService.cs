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

        public async Task<List<LocationResponse>> GetAllAsync()
        {
            var list = await _locationRepository.GetAllAsync();
            return list.Adapt<List<LocationResponse>>();
        }

        public async Task<LocationResponse?> GetByIdAsync(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            return location?.Adapt<LocationResponse>();
        }

        public async Task<int> CreateAsync(LocationRequest request)
        {
            if (request == null)
                return 0;

            var entity = request.Adapt<Location>();

            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = Status.Active;

            await _locationRepository.AddAsync(entity);
            await _locationRepository.SaveAsync();

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(int id, LocationRequest request)
        {
            var entity = await _locationRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            request.Adapt(entity);

            await _locationRepository.UpdateAsync(entity);
            await _locationRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _locationRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _locationRepository.DeleteAsync(entity);
            await _locationRepository.SaveAsync();

            return true;
        }
    }
}
