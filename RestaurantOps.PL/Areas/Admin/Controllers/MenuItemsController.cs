using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

    
        [HttpGet]
        public ActionResult<List<MenuItemResponse>> GetAll()
        {
            var items = _menuItemService.GetAll();
            return Ok(items);
        }

        
        [HttpGet("{id:int}")]
        public ActionResult<MenuItemResponse> GetById(int id)
        {
            var item = _menuItemService.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create(MenuItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _menuItemService.Create(request);
            if (!success)
                return BadRequest("Unable to create menu item.");

            return Ok("Menu item created successfully.");
        }

      
        [HttpPut("{id:int}")]
        public ActionResult Update(int id, MenuItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _menuItemService.Update(id, request);
            if (!success)
                return NotFound();

            return Ok("Menu item updated.");
        }

       
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var success = _menuItemService.Delete(id);
            if (!success)
                return NotFound();

            return Ok("Menu item deleted.");
        }
    }
}
