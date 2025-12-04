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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierResponse>>> GetAll()
        {
            var list = await _supplierService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierResponse>> GetById(int id)
        {
            var item = await _supplierService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SupplierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _supplierService.CreateAsync(request);
            if (id == 0)
                return BadRequest("Unable to create supplier.");

            return Ok(new { SupplierId = id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] SupplierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _supplierService.UpdateAsync(id, request);
            if (!ok)
                return NotFound();

            return Ok("Supplier updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _supplierService.DeleteAsync(id);
            if (!ok)
                return NotFound();

            return Ok("Supplier deleted.");
        }
    }
}
