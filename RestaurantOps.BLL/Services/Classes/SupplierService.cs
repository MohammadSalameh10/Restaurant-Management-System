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

        public async Task<List<SupplierResponse>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return suppliers.Adapt<List<SupplierResponse>>();
        }

        public async Task<SupplierResponse?> GetByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier?.Adapt<SupplierResponse>();
        }

        public async Task<int> CreateAsync(SupplierRequest request)
        {
            if (request == null)
                return 0;

            var location = await _locationRepository.GetByIdAsync(request.LocationId);
            if (location == null)
                return 0;

            var entity = request.Adapt<Supplier>();
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = Status.Active;

            await _supplierRepository.AddAsync(entity);
            await _supplierRepository.SaveAsync();

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(int id, SupplierRequest request)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                return false;

            var location = await _locationRepository.GetByIdAsync(request.LocationId);
            if (location == null)
                return false;

            request.Adapt(supplier);

            await _supplierRepository.UpdateAsync(supplier);
            await _supplierRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                return false;

            await _supplierRepository.DeleteAsync(supplier);
            await _supplierRepository.SaveAsync();

            return true;
        }
    }
}
