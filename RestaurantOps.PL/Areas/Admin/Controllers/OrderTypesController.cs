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
        public async Task<ActionResult<List<OrderTypeResponse>>> GetAll()
        {
            var list = await _orderTypeService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderTypeResponse>> GetById(int id)
        {
            var item = await _orderTypeService.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OrderTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _orderTypeService.CreateAsync(request);
            if (!ok) return BadRequest("Unable to create order type.");

            return Ok("Order type created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] OrderTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _orderTypeService.UpdateAsync(id, request);
            if (!ok) return NotFound();

            return Ok("Order type updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _orderTypeService.DeleteAsync(id);
            if (!ok) return NotFound();

            return Ok("Order type deleted.");
        }
    }
}
