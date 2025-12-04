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
        public async Task<ActionResult<List<InventoryItemResponse>>> GetAll()
        {
            var items = await _inventoryItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItemResponse>> GetById(int id)
        {
            var item = await _inventoryItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InventoryItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _inventoryItemService.CreateAsync(request);
            if (!success)
                return BadRequest("Unable to create inventory item.");

            return Ok("Inventory item created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] InventoryItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _inventoryItemService.UpdateAsync(id, request);
            if (!success)
                return NotFound();

            return Ok("Inventory item updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _inventoryItemService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Inventory item deleted.");
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<List<InventoryItemResponse>>> GetLowStock([FromQuery] decimal threshold = 10)
        {
            var items = await _inventoryItemService.GetLowStockAsync(threshold);
            return Ok(items);
        }
    }
}
