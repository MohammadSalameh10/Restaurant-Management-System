using Microsoft.AspNetCore.Http;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;

namespace RestaurantOps.BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        List<PaymentResponse> GetAll();
        PaymentResponse GetById(int id);
        PaymentResponse GetByOrderId(int orderId);
        int Create(PaymentRequest request);
        bool Update(int id, PaymentRequest request);
        bool Delete(int id);

        Task<OrderPaymentResponse> ProcessOrderPaymentAsync(OrderPaymentRequest request, string UserId, HttpRequest httpRequest);
        Task<bool> HandleVisaPaymentSuccessAsync(int orderId);
    }
}
