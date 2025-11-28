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
        public ActionResult<List<EmployeeResponse>> GetAll()
        {
            var list = _employeeService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<EmployeeResponse> GetById(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public ActionResult Create([FromBody] EmployeeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _employeeService.Create(request);
            if (!ok) return BadRequest("Unable to create employee.");

            return Ok("Employee created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] EmployeeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _employeeService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Employee updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _employeeService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Employee deleted.");
        }
    }
}
