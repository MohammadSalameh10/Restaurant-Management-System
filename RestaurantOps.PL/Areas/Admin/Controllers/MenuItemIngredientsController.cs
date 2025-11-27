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
    public class MenuItemIngredientsController : ControllerBase
    {
        private readonly IMenuItemIngredientService _menuItemIngredientService;

        public MenuItemIngredientsController(IMenuItemIngredientService menuItemIngredientService)
        {
            _menuItemIngredientService = menuItemIngredientService;
        }

        [HttpGet]
        public ActionResult<List<MenuItemIngredientResponse>> GetAll()
        {
            var list = _menuItemIngredientService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<MenuItemIngredientResponse> GetById(int id)
        {
            var item = _menuItemIngredientService.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] MenuItemIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = _menuItemIngredientService.Create(request);
            if (!ok)
                return BadRequest("Unable to create menu item ingredient.");

            return Ok("Menu item ingredient created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] MenuItemIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = _menuItemIngredientService.Update(id, request);
            if (!ok)
                return NotFound();

            return Ok("Menu item ingredient updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _menuItemIngredientService.Delete(id);
            if (!ok)
                return NotFound();

            return Ok("Menu item ingredient deleted.");
        }
    }
}
