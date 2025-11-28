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

        public List<OrderResponse> GetAll()
        {
            var orders = _orderRepository.GetAllWithDetails();
            return orders.Select(MapToResponse).ToList();
        }

        public OrderResponse GetById(int id)
        {
            var order = _orderRepository.GetOrderWithDetails(id);
            if (order == null)
                return null;

            return MapToResponse(order);
        }

        public int CreateOrder(OrderCreateRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return 0;

            var menuItemIds = request.Items.Select(i => i.MenuItemId).Distinct().ToList();
            var allMenuItems = _menuItemRepository.GetByIdsWithIngredients(menuItemIds);

            var requiredInventory = new Dictionary<int, decimal>();

            foreach (var orderItem in request.Items)
            {
                var menuItem = allMenuItems.FirstOrDefault(m => m.Id == orderItem.MenuItemId);
                if (menuItem == null)
                    return 0;

                foreach (var ingredient in menuItem.Ingredients)
                {
                    var neededForThisItem = ingredient.Quantity * orderItem.Quantity;

                    if (requiredInventory.ContainsKey(ingredient.InventoryItemId))
                    {
                        requiredInventory[ingredient.InventoryItemId] += neededForThisItem;
                    }
                    else
                    {
                        requiredInventory[ingredient.InventoryItemId] = neededForThisItem;
                    }
                }
            }

            var inventoryItems = _inventoryItemRepository.GetAll()
                .Where(i => requiredInventory.Keys.Contains(i.Id))
                .ToList();

            foreach (var inventoryItem in inventoryItems)
            {
                var needed = requiredInventory[inventoryItem.Id];
                if (inventoryItem.Stock < needed)
                {
                    return 0;
                }
            }

            foreach (var inventoryItem in inventoryItems)
            {
                var needed = requiredInventory[inventoryItem.Id];
                inventoryItem.Stock -= needed;
                _inventoryItemRepository.Update(inventoryItem);
            }
            _inventoryItemRepository.Save();

            var order = new Order
            {
                CustomerId = request.CustomerId,
                EmployeeId = request.EmployeeId,
                OrderTypeId = request.OrderTypeId,
                OrderStatusEnum = OrderStatus.Pending,
                Date = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                status = Status.Active,
                OrderItems = request.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    CreatedAt = DateTime.UtcNow,
                    status = Status.Active
                }).ToList()
            };

            _orderRepository.Add(order);
            _orderRepository.Save();

            return order.Id;
        }

        public bool ChangeStatus(int id, OrderStatus newStatus)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
                return false;

            order.OrderStatusEnum = newStatus;
            _orderRepository.Update(order);
            _orderRepository.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
                return false;

            _orderRepository.Delete(order);
            _orderRepository.Save();

            return true;
        }

        private OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Date = order.Date,
                Customer = order.Customer?.Name,
                Employee = order.Employee?.Name,
                Status = order.OrderStatusEnum.ToString(),
                OrderType = order.OrderType?.Name,
                Items = order.OrderItems?.Select(oi => new OrderItemResponse
                {
                    MenuItem = oi.MenuItem?.ItemName,
                    Quantity = oi.Quantity
                }).ToList() ?? new List<OrderItemResponse>()
            };
        }

        public List<OrderResponse> GetCustomerOrders(string userId)
        {
            var orders = _orderRepository.GetAllWithDetails()
                .Where(o => o.Customer != null && o.Customer.UserId == userId)
                .ToList();

            return orders.Select(MapToResponse).ToList();
        }

        public bool CancelOrderForCustomer(int orderId, string userId)
        {
            var order = _orderRepository.GetOrderWithDetails(orderId);
            if (order == null)
                return false;

            if (order.Customer == null || order.Customer.UserId != userId)
                return false;

            if (order.OrderStatusEnum != OrderStatus.Pending)
                return false;

            order.OrderStatusEnum = OrderStatus.Canceled;
            _orderRepository.Update(order);
            _orderRepository.Save();

            return true;
        }
    }
}
