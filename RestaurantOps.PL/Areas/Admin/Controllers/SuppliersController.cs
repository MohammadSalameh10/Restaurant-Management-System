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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public ActionResult<List<SupplierResponse>> GetAll()
        {
            var list = _supplierService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<SupplierResponse> GetById(int id)
        {
            var item = _supplierService.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] SupplierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _supplierService.Create(request);
            if (id == 0)
                return BadRequest("Unable to create supplier.");

            return Ok(new { SupplierId = id });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] SupplierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = _supplierService.Update(id, request);
            if (!ok)
                return NotFound();

            return Ok("Supplier updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _supplierService.Delete(id);
            if (!ok)
                return NotFound();

            return Ok("Supplier deleted.");
        }
    }
}
