using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IReportService
    {
        SalesReportResponse GetSalesReport();
    }
}
