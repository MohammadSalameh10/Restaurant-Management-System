using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Repositories.Interfaces
{
    public interface IInventoryOrderRepository
    {
        List<InventoryOrder> GetAll();
        InventoryOrder GetById(int id);
        void Add(InventoryOrder order);
        void Update(InventoryOrder order);
        void Delete(InventoryOrder order);
        void Save();
    }
}
