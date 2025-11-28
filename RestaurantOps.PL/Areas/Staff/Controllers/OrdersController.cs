using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.PL.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("today")]
        public ActionResult<List<OrderResponse>> GetTodayOrders()
        {
            var today = DateTime.UtcNow.Date;

            var orders = _orderService.GetAll()
                .Where(o => o.Date.Date == today)
                .ToList();

            return Ok(orders);
        }

        [HttpPatch("{id}/status")]
        public ActionResult ChangeStatus(int id, [FromBody] ChangeOrderStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!Enum.IsDefined(typeof(OrderStatus), request.OrderStatusId))
                return BadRequest("Invalid order status.");

            var newStatus = (OrderStatus)request.OrderStatusId;

            var success = _orderService.ChangeStatus(id, newStatus);
            if (!success)
                return NotFound();

            return Ok("Order status updated.");
        }
    }
}
