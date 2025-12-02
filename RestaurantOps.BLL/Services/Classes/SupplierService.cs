using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILocationRepository _locationRepository;

        public SupplierService(ISupplierRepository supplierRepository, ILocationRepository locationRepository)
        {
            _supplierRepository = supplierRepository;
            _locationRepository = locationRepository;
        }

        public List<SupplierResponse> GetAll()
        {
            var suppliers = _supplierRepository.GetAll();
            return suppliers.Adapt<List<SupplierResponse>>();
        }

        public SupplierResponse GetById(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            return supplier?.Adapt<SupplierResponse>();
        }

        public int Create(SupplierRequest request)
        {
            if (request == null)
                return 0;

            var location = _locationRepository.GetById(request.LocationId);
            if (location == null)
                return 0;

            var entity = request.Adapt<Supplier>();
            entity.CreatedAt = DateTime.UtcNow;
            entity.status = Status.Active;

            _supplierRepository.Add(entity);
            _supplierRepository.Save();

            return entity.Id;
        }

        public bool Update(int id, SupplierRequest request)
        {
            var supplier = _supplierRepository.GetById(id);
            if (supplier == null)
                return false;

            var location = _locationRepository.GetById(request.LocationId);
            if (location == null)
                return false;

            request.Adapt(supplier);

            _supplierRepository.Update(supplier);
            _supplierRepository.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            if (supplier == null)
                return false;

            _supplierRepository.Delete(supplier);
            _supplierRepository.Save();

            return true;
        }
    }
}
