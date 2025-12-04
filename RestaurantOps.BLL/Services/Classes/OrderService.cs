using Mapster;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IMenuItemRepository menuItemRepository,
            IInventoryItemRepository inventoryItemRepository)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            return order?.Adapt<OrderResponse>();
        }

        public async Task<int> CreateOrderAsync(OrderCreateRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return 0;

            var menuItemIds = request.Items
                .Select(i => i.MenuItemId)
                .Distinct()
                .ToList();

            var allMenuItems = await _menuItemRepository.GetByIdsWithIngredientsAsync(menuItemIds);

            if (allMenuItems.Count != menuItemIds.Count)
                return 0;

            var requiredInventory = new Dictionary<int, decimal>();

            foreach (var orderItem in request.Items)
            {
                var menuItem = allMenuItems.FirstOrDefault(m => m.Id == orderItem.MenuItemId);
                if (menuItem == null)
                    return 0;

                foreach (var ingredient in menuItem.Ingredients)
                {
                    var needed = ingredient.Quantity * orderItem.Quantity;

                    if (requiredInventory.ContainsKey(ingredient.InventoryItemId))
                        requiredInventory[ingredient.InventoryItemId] += needed;
                    else
                        requiredInventory[ingredient.InventoryItemId] = needed;
                }
            }

            var inventoryItems = (await _inventoryItemRepository.GetAllAsync())
                .Where(i => requiredInventory.Keys.Contains(i.Id))
                .ToList();

            foreach (var inventoryItem in inventoryItems)
            {
                var needed = requiredInventory[inventoryItem.Id];
                if (inventoryItem.Stock < needed)
                    return 0;
            }

            foreach (var inventoryItem in inventoryItems)
            {
                var needed = requiredInventory[inventoryItem.Id];
                inventoryItem.Stock -= needed;
                _inventoryItemRepository.UpdateAsync(inventoryItem);
            }

            await _inventoryItemRepository.SaveAsync();

            var orderItems = new List<OrderItem>();

            foreach (var itemRequest in request.Items)
            {
                var menuItem = allMenuItems.First(m => m.Id == itemRequest.MenuItemId);

                orderItems.Add(new OrderItem
                {
                    MenuItemId = itemRequest.MenuItemId,
                    Quantity = itemRequest.Quantity,
                    Price = menuItem.Price,
                    CreatedAt = DateTime.UtcNow,
                    Status = Status.Active
                });
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                EmployeeId = request.EmployeeId,
                OrderTypeId = request.OrderTypeId,
                OrderStatusEnum = OrderStatus.Pending,
                Date = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Status = Status.Active,
                OrderItems = orderItems,
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync();

            return order.Id;
        }

        public async Task<bool> ChangeStatusAsync(int id, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            order.OrderStatusEnum = newStatus;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            await _orderRepository.DeleteAsync(order);
            await _orderRepository.SaveAsync();

            return true;
        }

        public async Task<List<OrderResponse>> GetCustomerOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            var filtered = orders
                .Where(o => o.Customer != null && o.Customer.UserId == userId)
                .ToList();

            return filtered.Adapt<List<OrderResponse>>();
        }

        public async Task<bool> CancelOrderForCustomerAsync(int orderId, string userId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                return false;

            if (order.Customer == null || order.Customer.UserId != userId)
                return false;

            if (order.OrderStatusEnum != OrderStatus.Pending)
                return false;

            order.OrderStatusEnum = OrderStatus.Canceled;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            return true;
        }

        public async Task<List<OrderResponse>> GetOrdersForEmployeeAsync(int employeeId)
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            var list = orders.Where(o => o.EmployeeId == employeeId).ToList();

            return list.Adapt<List<OrderResponse>>();
        }
        public async Task<bool> AssignOrderToEmployeeAsync(int orderId, int employeeId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.EmployeeId = employeeId;

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            return true;
        }
    }
}
