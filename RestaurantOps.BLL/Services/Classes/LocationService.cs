using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return list.Select(l => new LocationResponse
            {
                Id = l.Id,
                City = l.City,
                Street = l.Street
            }).ToList();
        }

        public LocationResponse GetById(int id)
        {
            var l = _locationRepository.GetById(id);
            if (l == null)
                return null;

            return new LocationResponse
            {
                Id = l.Id,
                City = l.City,
                Street = l.Street
            };
        }

        public int Create(LocationRequest request)
        {
            if (request == null)
                return 0;

            var entity = new Location
            {
                City = request.City,
                Street = request.Street,
                CreatedAt = DateTime.UtcNow,
                status = Status.Active
            };

            _locationRepository.Add(entity);
            _locationRepository.Save();

            return entity.Id;
        }

        public bool Update(int id, LocationRequest request)
        {
            var entity = _locationRepository.GetById(id);
            if (entity == null)
                return false;

            entity.City = request.City;
            entity.Street = request.Street;

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
