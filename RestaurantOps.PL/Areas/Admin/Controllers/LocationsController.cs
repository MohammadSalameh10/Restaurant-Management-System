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
        public async Task<ActionResult<List<LocationResponse>>> GetAll()
        {
            var list = await _locationService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationResponse>> GetById(int id)
        {
            var item = await _locationService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _locationService.CreateAsync(request);
            if (id == 0)
                return BadRequest("Unable to create location.");

            return Ok(new { LocationId = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _locationService.UpdateAsync(id, request);
            if (!ok)
                return NotFound();

            return Ok("Location updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _locationService.DeleteAsync(id);
            if (!ok)
                return NotFound();

            return Ok("Location deleted.");
        }
    }
}
