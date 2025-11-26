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
    public class OrderStatusesController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusesController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        [HttpGet]
        public ActionResult<List<OrderStatusResponse>> GetAll()
        {
            var list = _orderStatusService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderStatusResponse> GetById(int id)
        {
            var item = _orderStatusService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] OrderStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderStatusService.Create(request);
            if (!ok) return BadRequest("Unable to create order status.");

            return Ok("Order status created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] OrderStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderStatusService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Order status updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _orderStatusService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Order status deleted.");
        }
    }
}
