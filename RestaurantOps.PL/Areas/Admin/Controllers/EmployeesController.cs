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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
        {
            var list = await _employeeService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EmployeeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _employeeService.CreateAsync(request);
            if (!ok) return BadRequest("Unable to create employee.");

            return Ok("Employee created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EmployeeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _employeeService.UpdateAsync(id, request);
            if (!ok) return NotFound();

            return Ok("Employee updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _employeeService.DeleteAsync(id);
            if (!ok) return NotFound();

            return Ok("Employee deleted.");
        }
    }
}
