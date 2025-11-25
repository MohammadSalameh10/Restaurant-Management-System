using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public List<InventoryItemResponse> GetAll()
        {
            var items = _inventoryItemRepository.GetAll();

            return items.Select(MapToResponse).ToList();
        }

        public InventoryItemResponse GetById(int id)
        {
            var item = _inventoryItemRepository.GetById(id);
            if (item == null)
                return null;

            return MapToResponse(item);
        }

        public bool Create(InventoryItemRequest request)
        {
            if (request == null)
                return false;

            var entity = new InventoryItem
            {
                Name = request.Name,
                Stock = request.Stock,
                SupplierId = request.SupplierId
            };

            _inventoryItemRepository.Add(entity);
            _inventoryItemRepository.Save();
            return true;
        }

        public bool Update(int id, InventoryItemRequest request)
        {
            var item = _inventoryItemRepository.GetById(id);
            if (item == null)
                return false;

            item.Name = request.Name;
            item.Stock = request.Stock;
            item.SupplierId = request.SupplierId;

            _inventoryItemRepository.Update(item);
            _inventoryItemRepository.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var item = _inventoryItemRepository.GetById(id);
            if (item == null)
                return false;

            _inventoryItemRepository.Delete(item);
            _inventoryItemRepository.Save();
            return true;
        }

        public List<InventoryItemResponse> GetLowStock(decimal threshold)
        {
            var items = _inventoryItemRepository.GetLowStockItems(threshold);
            return items.Select(MapToResponse).ToList();
        }

        private InventoryItemResponse MapToResponse(InventoryItem item)
        {
            return new InventoryItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Stock = item.Stock,
                SupplierId = item.SupplierId,
                SupplierName = item.Supplier?.Name
            };
        }
    }
}
