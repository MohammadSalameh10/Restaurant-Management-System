using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost("process")]
        public async Task<ActionResult<OrderPaymentResponse>> Process([FromBody] OrderPaymentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var result = await _paymentService.ProcessOrderPaymentAsync(request, userId, Request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("success/{orderId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Success(int orderId)
        {
            var ok = await _paymentService.HandleVisaPaymentSuccessAsync(orderId);
            if (!ok)
                return BadRequest(new { Message = "Payment verification failed." });

            return Ok(new { Message = "Payment successful." });
        }

        [HttpGet("cancel/{orderId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Cancel(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
                return NotFound(new { Message = "Order not found." });

            if (order.Status == OrderStatus.Completed.ToString())
                return BadRequest(new { Message = "Order already completed. Cannot cancel payment." });

            var result = await _orderService.ChangeStatusAsync(orderId, OrderStatus.Canceled);

            if (!result)
                return BadRequest(new { Message = "Failed to cancel order." });

            return Ok(new { Message = "Payment canceled and order updated." });
        }
    }
}
