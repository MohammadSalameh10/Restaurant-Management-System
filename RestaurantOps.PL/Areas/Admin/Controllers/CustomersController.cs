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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _customerService.CreateAsync(request);
            if (!success)
                return BadRequest("Unable to create customer.");

            return Ok("Customer created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _customerService.UpdateAsync(id, request);
            if (!success)
                return NotFound();

            return Ok("Customer updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _customerService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok("Customer deleted.");
        }
    }
}
