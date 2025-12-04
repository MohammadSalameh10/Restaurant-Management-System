using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<OrderResponse>>> Filter(
            [FromQuery] int? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? customerName)
        {
            var orders = await _orderService.GetAllAsync();
            var query = orders.AsQueryable();

            if (status.HasValue && Enum.IsDefined(typeof(OrderStatus), status.Value))
            {
                var statusName = ((OrderStatus)status.Value).ToString();
                query = query.Where(o => o.Status == statusName);
            }

            if (fromDate.HasValue)
            {
                var from = fromDate.Value.Date;
                query = query.Where(o => o.Date.Date >= from);
            }

            if (toDate.HasValue)
            {
                var to = toDate.Value.Date;
                query = query.Where(o => o.Date.Date <= to);
            }

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                var name = customerName.Trim().ToLower();
                query = query.Where(o =>
                    !string.IsNullOrEmpty(o.Customer) &&
                    o.Customer.ToLower().Contains(name));
            }

            var filtered = query.ToList();
            return Ok(filtered);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<OrderResponse>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var orders = await _orderService.GetAllAsync();
            var totalCount = orders.Count;

            var items = orders
                .OrderByDescending(o => o.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<OrderResponse>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> ChangeStatus(int id, [FromBody] ChangeOrderStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!Enum.IsDefined(typeof(OrderStatus), request.OrderStatusId))
                return BadRequest("Invalid order status.");

            var newStatus = (OrderStatus)request.OrderStatusId;

            var success = await _orderService.ChangeStatusAsync(id, newStatus);
            if (!success)
                return NotFound();

            return Ok("Order status updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _orderService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Order deleted.");
        }
    }
}
