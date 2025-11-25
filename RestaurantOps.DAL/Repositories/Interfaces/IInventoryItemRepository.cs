using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IInventoryItemRepository
    {
        List<InventoryItem> GetAll();
        InventoryItem GetById(int id);
        void Add(InventoryItem item);
        void Update(InventoryItem item);
        void Delete(InventoryItem item);
        void Save();
        List<InventoryItem> GetLowStockItems(decimal threshold);
    }
}
