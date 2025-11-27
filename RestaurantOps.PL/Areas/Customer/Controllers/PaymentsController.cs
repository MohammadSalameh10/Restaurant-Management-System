using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
