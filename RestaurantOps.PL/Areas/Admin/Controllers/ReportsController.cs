using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales")]
        public IActionResult GetSalesReport()
        {
            var report = _reportService.GetSalesReport();
            return Ok(report);
        }

        [HttpGet("employees-performance")]
        public ActionResult<List<EmployeePerformanceResponse>> GetEmployeePerformance()
        {
            var report = _reportService.GetEmployeePerformanceReport();
            return Ok(report);
        }
    }
}
