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
    public class MenuItemIngredientsController : ControllerBase
    {
        private readonly IMenuItemIngredientService _menuItemIngredientService;

        public MenuItemIngredientsController(IMenuItemIngredientService menuItemIngredientService)
        {
            _menuItemIngredientService = menuItemIngredientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MenuItemIngredientResponse>>> GetAll()
        {
            var list = await _menuItemIngredientService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemIngredientResponse>> GetById(int id)
        {
            var item = await _menuItemIngredientService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MenuItemIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _menuItemIngredientService.CreateAsync(request);
            if (!ok)
                return BadRequest("Unable to create menu item ingredient.");

            return Ok("Menu item ingredient created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] MenuItemIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _menuItemIngredientService.UpdateAsync(id, request);
            if (!ok)
                return NotFound();

            return Ok("Menu item ingredient updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _menuItemIngredientService.DeleteAsync(id);
            if (!ok)
                return NotFound();

            return Ok("Menu item ingredient deleted.");
        }
    }
}
