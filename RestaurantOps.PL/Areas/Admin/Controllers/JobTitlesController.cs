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
    public class JobTitlesController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;

        public JobTitlesController(IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JobTitleResponse>>> GetAll()
        {
            var list = await _jobTitleService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobTitleResponse>> GetById(int id)
        {
            var item = await _jobTitleService.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] JobTitleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _jobTitleService.CreateAsync(request);
            if (!ok) return BadRequest("Unable to create job title.");

            return Ok("Job title created.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] JobTitleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _jobTitleService.UpdateAsync(id, request);
            if (!ok) return NotFound();

            return Ok("Job title updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _jobTitleService.DeleteAsync(id);
            if (!ok) return NotFound();

            return Ok("Job title deleted.");
        }
    }
}
