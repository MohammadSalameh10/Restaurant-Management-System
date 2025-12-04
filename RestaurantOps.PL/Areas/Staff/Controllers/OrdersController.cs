using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.PL.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmployeeRepository _employeeRepository;

        public OrdersController(IOrderService orderService, IEmployeeRepository employeeRepository)
        {
            _orderService = orderService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("today")]
        public async Task<ActionResult<List<OrderResponse>>> GetTodayOrders()
        {
            var today = DateTime.UtcNow.Date;

            var orders = await _orderService.GetAllAsync();
            var todayOrders = orders
                .Where(o => o.Date.Date == today)
                .ToList();

            return Ok(todayOrders);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<OrderResponse>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            if (employee == null)
                return BadRequest("Employee profile not found for this user.");

            var orders = await _orderService.GetOrdersForEmployeeAsync(employee.Id);
            return Ok(orders);
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

        [HttpPatch("{id}/assign-to-me")]
        public async Task<ActionResult> AssignToMe(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            if (employee == null)
                return BadRequest("Employee profile not found for this user.");

            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            var success = await _orderService.AssignOrderToEmployeeAsync(id, employee.Id);
            if (!success)
                return BadRequest("Unable to assign this order.");

            return Ok(new { Message = "Order assigned to you successfully." });
        }
    }
}
