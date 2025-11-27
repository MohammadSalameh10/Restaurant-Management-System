using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
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
    }
}
