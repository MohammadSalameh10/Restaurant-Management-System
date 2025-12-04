using Mapster;
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

        public async Task<List<InventoryItemResponse>> GetAllAsync()
        {
            var items = await _inventoryItemRepository.GetAllAsync();
            return items.Adapt<List<InventoryItemResponse>>();
        }

        public async Task<InventoryItemResponse?> GetByIdAsync(int id)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(id);
            return item?.Adapt<InventoryItemResponse>();
        }

        public async Task<bool> CreateAsync(InventoryItemRequest request)
        {
            if (request == null)
                return false;

            var entity = new InventoryItem
            {
                Name = request.Name,
                Stock = request.Stock,
                SupplierId = request.SupplierId
            };

            await _inventoryItemRepository.AddAsync(entity);
            await _inventoryItemRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, InventoryItemRequest request)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(id);
            if (item == null)
                return false;

            item.Name = request.Name;
            item.Stock = request.Stock;
            item.SupplierId = request.SupplierId;

            await _inventoryItemRepository.UpdateAsync(item);
            await _inventoryItemRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(id);
            if (item == null)
                return false;

            await _inventoryItemRepository.DeleteAsync(item);
            await _inventoryItemRepository.SaveAsync();

            return true;
        }

        public async Task<List<InventoryItemResponse>> GetLowStockAsync(decimal threshold)
        {
            var items = await _inventoryItemRepository.GetLowStockItemsAsync(threshold);
            return items.Adapt<List<InventoryItemResponse>>();
        }
    }
}
