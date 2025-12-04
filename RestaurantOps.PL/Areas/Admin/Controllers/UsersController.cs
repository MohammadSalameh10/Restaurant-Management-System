using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPatch("{id}/role")]
        public async Task<ActionResult> ChangeRole(string id, [FromBody] ChangeUserRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NewRole))
                return BadRequest(new { Message = "NewRole is required." });

            var ok = await _authenticationService.ChangeUserRoleAsync(id, request);
            if (!ok)
                return BadRequest(new { Message = "Failed to change user role." });

            return Ok(new { Message = "User role updated successfully." });
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<UserListResponse>>> GetAllUsers()
        {
            var users = await _authenticationService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
