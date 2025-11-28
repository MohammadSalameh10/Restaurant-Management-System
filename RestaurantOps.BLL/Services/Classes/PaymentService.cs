using Microsoft.AspNetCore.Http;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;
using Stripe.Checkout;

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
                OrderStatus = p.Order?.OrderStatusEnum.ToString(),
                CustomerName = p.Order?.Customer?.Name
            };
        }

        public async Task<OrderPaymentResponse> ProcessOrderPaymentAsync(OrderPaymentRequest request, string userId, HttpRequest httpRequest)
        {
            if (request == null)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Invalid request."
                };

            var order = _orderRepository.GetOrderWithDetails(request.OrderId);
            if (order == null)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Order not found."
                };

            if (order.CustomerId.ToString() != userId)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Not authorized to pay for this order."
                };

            if (order.OrderItems == null || !order.OrderItems.Any())
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Order has no items."
                };

            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                totalAmount += item.Quantity * item.Price;
            }

            if (totalAmount <= 0)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Invalid order amount."
                };

            var method = string.IsNullOrWhiteSpace(request.Method) ? "Cash" : request.Method.Trim();

            if (!method.Equals("Visa", StringComparison.OrdinalIgnoreCase))
            {
                var cashPayment = new Payment
                {
                    OrderId = order.Id,
                    Amount = totalAmount,
                    Method = method,
                    Provider = "Cash",
                    ProviderPaymentId = null,
                    PaidAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    status = Status.Active
                };

                _paymentRepository.Add(cashPayment);
                _paymentRepository.Save();

                order.OrderStatusEnum = OrderStatus.Completed;
                _orderRepository.Update(order);
                _orderRepository.Save();

                return new OrderPaymentResponse
                {
                    Success = true,
                    Message = "Cash payment completed successfully.",
                    Url = null,
                    PaymentId = null
                };
            }

            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{baseUrl}/api/Customer/Payments/success/{order.Id}",
                CancelUrl = $"{baseUrl}/api/Customer/Payments/cancel/{order.Id}"
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Order #{order.Id}"
                    },
                    UnitAmount = (long)(totalAmount * 100)
                },
                Quantity = 1
            });

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            order.OrderStatusEnum = OrderStatus.Preparing;
            _orderRepository.Update(order);
            _orderRepository.Save();

            return new OrderPaymentResponse
            {
                Success = true,
                Message = "Payment processed successfully.",
                Url = session.Url,
                PaymentId = session.Id
            };
        }

        public async Task<bool> HandleVisaPaymentSuccessAsync(int orderId)
        {
            var order = _orderRepository.GetOrderWithDetails(orderId);
            if (order == null)
                return false;

            if (order.OrderItems == null || !order.OrderItems.Any())
                return false;

            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                totalAmount += item.Quantity * item.Price;
            }

            if (totalAmount <= 0)
                return false;

            var existingPayment = _paymentRepository.GetByOrderId(order.Id);
            if (existingPayment != null)
                return true;

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = totalAmount,
                Method = "Visa",
                Provider = "Stripe",
                PaidAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                status = Status.Active
            };

            _paymentRepository.Add(payment);
            _paymentRepository.Save();

            order.OrderStatusEnum = OrderStatus.Completed;
            _orderRepository.Update(order);
            _orderRepository.Save();

            return true;
        }

    }
}
