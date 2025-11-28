using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrdersController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public ActionResult CreateOrder([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _orderService.CreateOrder(request);
            if (id == 0)
                return BadRequest(new { Message = "Invalid request." });

            return Ok(new { OrderId = id });
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> GetById(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{id}/pay")]
        public async Task<ActionResult> PayOrder(int id, [FromBody] string method)
        {
            if (string.IsNullOrWhiteSpace(method))
                method = "Cash";

            var request = new OrderPaymentRequest
            {
                OrderId = id,
                Method = method
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _paymentService.ProcessOrderPaymentAsync(request, userId, Request);
            if (!response.Success)
                return BadRequest(new { response.Message });

            return Ok(response);
        }

        [HttpGet("my")]
        public ActionResult<List<OrderResponse>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var orders = _orderService.GetCustomerOrders(userId);
            return Ok(orders);
        }

        [HttpPatch("{id}/cancel")]
        public ActionResult CancelMyOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = _orderService.CancelOrderForCustomer(id, userId);
            if (!success)
                return BadRequest("Unable to cancel this order.");

            return Ok("Order canceled successfully.");
        }

    }
}
