using Microsoft.AspNetCore.Authorization;
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
    public class OrderTypesController : ControllerBase
    {
        private readonly IOrderTypeService _orderTypeService;

        public OrderTypesController(IOrderTypeService orderTypeService)
        {
            _orderTypeService = orderTypeService;
        }

        [HttpGet]
        public ActionResult<List<OrderTypeResponse>> GetAll()
        {
            var list = _orderTypeService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderTypeResponse> GetById(int id)
        {
            var item = _orderTypeService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] OrderTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderTypeService.Create(request);
            if (!ok) return BadRequest("Unable to create order type.");

            return Ok("Order type created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] OrderTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderTypeService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Order type updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _orderTypeService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Order type deleted.");
        }
    }
}
