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
    public class InventoryOrdersController : ControllerBase
    {
        private readonly IInventoryOrderService _inventoryOrderService;

        public InventoryOrdersController(IInventoryOrderService inventoryOrderService)
        {
            _inventoryOrderService = inventoryOrderService;
        }

        [HttpGet]
        public ActionResult<List<InventoryOrderResponse>> GetAll()
        {
            var list = _inventoryOrderService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<InventoryOrderResponse> GetById(int id)
        {
            var item = _inventoryOrderService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] InventoryOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = _inventoryOrderService.Create(request);
            if (id == 0)
                return BadRequest("Unable to create inventory order (check items & inventory items).");

            return Ok(new { OrderId = id });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _inventoryOrderService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Inventory order deleted.");
        }
    }
}
