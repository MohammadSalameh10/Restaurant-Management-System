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
    public class InventoryItemsController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemsController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        [HttpGet]
        public ActionResult<List<InventoryItemResponse>> GetAll()
        {
            var items = _inventoryItemService.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<InventoryItemResponse> GetById(int id)
        {
            var item = _inventoryItemService.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] InventoryItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _inventoryItemService.Create(request);
            if (!success)
                return BadRequest("Unable to create inventory item.");

            return Ok("Inventory item created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] InventoryItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _inventoryItemService.Update(id, request);
            if (!success)
                return NotFound();

            return Ok("Inventory item updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _inventoryItemService.Delete(id);
            if (!success)
                return NotFound();

            return Ok("Inventory item deleted.");
        }

        [HttpGet("low-stock")]
        public ActionResult<List<InventoryItemResponse>> GetLowStock([FromQuery] decimal threshold = 10)
        {
            var items = _inventoryItemService.GetLowStock(threshold);
            return Ok(items);
        }
    }
}
