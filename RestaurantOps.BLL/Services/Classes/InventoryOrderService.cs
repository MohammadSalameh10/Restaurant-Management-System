using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class InventoryOrderService : IInventoryOrderService
    {
        private readonly IInventoryOrderRepository _inventoryOrderRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryOrderService(
            IInventoryOrderRepository inventoryOrderRepository,
            IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryOrderRepository = inventoryOrderRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public List<InventoryOrderResponse> GetAll()
        {
            var orders = _inventoryOrderRepository.GetAll();
            return orders.Select(MapToResponse).ToList();
        }

        public InventoryOrderResponse GetById(int id)
        {
            var order = _inventoryOrderRepository.GetById(id);
            if (order == null) return null;

            return MapToResponse(order);
        }

        public int Create(InventoryOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return 0;

            var inventoryItemIds = request.Items
                .Select(i => i.InventoryItemId)
                .Distinct()
                .ToList();

            var inventoryItems = _inventoryItemRepository.GetAll()
                .Where(i => inventoryItemIds.Contains(i.Id))
                .ToList();

            if (inventoryItems.Count != inventoryItemIds.Count)
                return 0;

            var order = new InventoryOrder
            {
                Date = request.Date,
                EmployeeId = request.EmployeeId,
                Items = request.Items.Select(i => new InventoryOrderItem
                {
                    InventoryItemId = i.InventoryItemId,
                    Stock = i.Stock,
                    Price = i.Price,
                    CreatedAt = DateTime.UtcNow,
                    status = Status.Active
                }).ToList()
            };

            foreach (var orderItem in request.Items)
            {
                var invItem = inventoryItems.First(x => x.Id == orderItem.InventoryItemId);
                invItem.Stock += orderItem.Stock;  
                _inventoryItemRepository.Update(invItem);
            }
            _inventoryItemRepository.Save();

         
            _inventoryOrderRepository.Add(order);
            _inventoryOrderRepository.Save();

            return order.Id;
        }

        public bool Delete(int id)
        {
            var order = _inventoryOrderRepository.GetById(id);
            if (order == null) return false;

            _inventoryOrderRepository.Delete(order);
            _inventoryOrderRepository.Save();
            return true;
        }

        private InventoryOrderResponse MapToResponse(InventoryOrder o)
        {
            var items = o.Items ?? new List<InventoryOrderItem>();

            var itemsDto = items.Select(i => new InventoryOrderItemResponse
            {
                InventoryItemName = i.InventoryItem?.Name,
                Stock = i.Stock,
                Price = i.Price
            }).ToList();

            var totalCost = itemsDto.Sum(i => i.Stock * i.Price);

            return new InventoryOrderResponse
            {
                Id = o.Id,
                Date = o.Date,
                EmployeeId = o.EmployeeId,
                EmployeeName = o.Employee?.Name,
                TotalCost = totalCost,
                Items = itemsDto
            };
        }
    }
}
