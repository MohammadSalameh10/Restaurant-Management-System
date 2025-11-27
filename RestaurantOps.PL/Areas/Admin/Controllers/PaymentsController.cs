using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public ActionResult<List<PaymentResponse>> GetAll()
        {
            var list = _paymentService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<PaymentResponse> GetById(int id)
        {
            var item = _paymentService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpGet("by-order/{orderId:int}")]
        public ActionResult<PaymentResponse> GetByOrderId(int orderId)
        {
            var item = _paymentService.GetByOrderId(orderId);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = _paymentService.Create(request);
            if (id == 0)
                return BadRequest("Unable to create payment. Check order or existing payment.");

            return Ok(new { PaymentId = id });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _paymentService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Payment updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _paymentService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Payment deleted.");
        }
    }
}
