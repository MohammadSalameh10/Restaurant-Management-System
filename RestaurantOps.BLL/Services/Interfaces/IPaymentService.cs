using Microsoft.AspNetCore.Http;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<OrderPaymentResponse> ProcessOrderPaymentAsync(OrderPaymentRequest request, string UserId, HttpRequest httpRequest);
        Task<bool> HandleVisaPaymentSuccessAsync(int orderId);
    }
}
