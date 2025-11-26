using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public List<PaymentResponse> GetAll()
        {
            var payments = _paymentRepository.GetAll();
            return payments.Select(MapToResponse).ToList();
        }

        public PaymentResponse GetById(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) return null;

            return MapToResponse(payment);
        }

        public PaymentResponse GetByOrderId(int orderId)
        {
            var payment = _paymentRepository.GetByOrderId(orderId);
            if (payment == null) return null;

            return MapToResponse(payment);
        }

        public int Create(PaymentRequest request)
        {
            if (request == null) return 0;

            var order = _orderRepository.GetById(request.OrderId);
            if (order == null) return 0;

            var existing = _paymentRepository.GetByOrderId(request.OrderId);
            if (existing != null) return 0;

            var entity = new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                Method = request.Method,
                PaidAt = request.PaidAt
            };

            _paymentRepository.Add(entity);
            _paymentRepository.Save();

            return entity.Id;
        }

        public bool Update(int id, PaymentRequest request)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) return false;

            payment.Amount = request.Amount;
            payment.Method = request.Method;
            payment.PaidAt = request.PaidAt;

            _paymentRepository.Update(payment);
            _paymentRepository.Save();
            return true;
        }

        public bool Delete(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            _paymentRepository.Save();
            return true;
        }

        private PaymentResponse MapToResponse(Payment p)
        {
            return new PaymentResponse
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                Method = p.Method,
                PaidAt = p.PaidAt,
                OrderStatus = p.Order?.OrderStatus?.Name,
                CustomerName = p.Order?.Customer?.Name
            };
        }
    }
}
