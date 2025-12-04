using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Classes;

namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportPdfService _reportPdfService;

        public ReportsController(ReportPdfService reportPdfService)
        {
            _reportPdfService = reportPdfService;
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport()
        {
            var pdf = await _reportPdfService.GenerateSalesReportPdfAsync();

            return File(
                pdf,
                "application/pdf",
                $"SalesReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf"
            );
        }
    }
}
