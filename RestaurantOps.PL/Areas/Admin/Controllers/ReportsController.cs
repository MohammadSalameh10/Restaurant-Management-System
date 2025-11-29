using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Classes;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Responses;
using QuestPDF.Fluent;


namespace RestaurantOps.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ReportPdfService _reportPdfService;

        public ReportsController(IReportService reportService, ReportPdfService reportPdfService)
        {
            _reportService = reportService;
            _reportPdfService = reportPdfService;
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

        [HttpGet("sales-pdf")]
        public ActionResult GetSalesReportPdf()
        {
            var document = _reportPdfService.CreateSalesReportDocument();
            var pdfBytes = document.GeneratePdf();

            var fileName = $"SalesReport_{DateTime.UtcNow:yyyyMMdd_HHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
