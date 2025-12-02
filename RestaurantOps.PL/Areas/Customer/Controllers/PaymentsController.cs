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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
        public ActionResult Cancel(int orderId)
        {
            return Ok(new { Message = "Payment canceled." });
        }
    }
}
