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
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MenuItemResponse>>> GetAll()
        {
            var items = await _menuItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemResponse>> GetById(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MenuItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _menuItemService.CreateAsync(request);
            if (!success)
                return BadRequest("Unable to create menu item.");

            return Ok("Menu item created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] MenuItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _menuItemService.UpdateAsync(id, request);
            if (!success)
                return NotFound();

            return Ok("Menu item updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _menuItemService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Menu item deleted.");
        }
    }
}
