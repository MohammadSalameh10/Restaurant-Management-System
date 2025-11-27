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
        public ActionResult<List<CustomerResponse>> GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerResponse> GetById(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _customerService.Create(request);
            if (!success)
                return BadRequest("Unable to create customer.");

            return Ok("Customer created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _customerService.Update(id, request);
            if (!success)
                return NotFound();

            return Ok("Customer updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = _customerService.Delete(id);
            if (!success)
                return NotFound();

            return Ok("Customer deleted.");
        }
    }
}
