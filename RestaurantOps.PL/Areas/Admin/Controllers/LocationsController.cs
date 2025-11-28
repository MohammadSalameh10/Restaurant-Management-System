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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public ActionResult<List<LocationResponse>> GetAll()
        {
            var list = _locationService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<LocationResponse> GetById(int id)
        {
            var item = _locationService.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _locationService.Create(request);
            if (id == 0)
                return BadRequest("Unable to create location.");

            return Ok(new { LocationId = id });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = _locationService.Update(id, request);
            if (!ok)
                return NotFound();

            return Ok("Location updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _locationService.Delete(id);
            if (!ok)
                return NotFound();

            return Ok("Location deleted.");
        }
    }
}
