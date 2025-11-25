using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
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
    }
}
