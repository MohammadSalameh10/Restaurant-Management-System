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
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public ActionResult<List<ShiftResponse>> GetAll()
        {
            var list = _shiftService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<ShiftResponse> GetById(int id)
        {
            var item = _shiftService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ShiftRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _shiftService.Create(request);
            if (!ok) return BadRequest("Unable to create shift.");

            return Ok("Shift created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] ShiftRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _shiftService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Shift updated.");
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var ok = _shiftService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Shift deleted.");
        }

        [HttpGet("employee/{employeeId}")]
        public ActionResult<List<ShiftResponse>> GetEmployeeShifts(
            int employeeId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var list = _shiftService.GetEmployeeShifts(employeeId, from, to);
            return Ok(list);
        }
    }
}
