using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet("today")]
        public ActionResult<List<ShiftResponse>> GetTodayShifts()
        {
            var today = DateTime.UtcNow.Date;

            var shifts = _shiftService.GetAll()
                .Where(s => s.ExpectedCheckIn.Date == today)
                .ToList();

            return Ok(shifts);
        }

        [HttpPost("checkin")]
        public ActionResult CheckIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var ok = _shiftService.EmployeeCheckIn(userId);
            if (!ok)
                return BadRequest("No shift found for today or already checked in.");

            return Ok("Check-in successful.");
        }

        [HttpPost("checkout")]
        public ActionResult CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var ok = _shiftService.EmployeeCheckOut(userId);
            if (!ok)
                return BadRequest("No active shift found for checkout.");

            return Ok("Check-out successful.");
        }
    }
}
