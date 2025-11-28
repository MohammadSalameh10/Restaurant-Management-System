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
        public ActionResult<List<JobTitleResponse>> GetAll()
        {
            var list = _jobTitleService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<JobTitleResponse> GetById(int id)
        {
            var item = _jobTitleService.GetById(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Create([FromBody] JobTitleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _jobTitleService.Create(request);
            if (!ok) return BadRequest("Unable to create job title.");

            return Ok("Job title created.");
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] JobTitleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _jobTitleService.Update(id, request);
            if (!ok) return NotFound();

            return Ok("Job title updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ok = _jobTitleService.Delete(id);
            if (!ok) return NotFound();

            return Ok("Job title deleted.");
        }
    }
}
