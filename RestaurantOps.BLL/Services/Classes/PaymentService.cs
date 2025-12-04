using Mapster;
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

        public async Task<OrderPaymentResponse> ProcessOrderPaymentAsync(
            OrderPaymentRequest request,
            string userId,
            HttpRequest httpRequest)
        {
            if (request == null)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Invalid request."
                };

            var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId);
            if (order == null)
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "Order not found."
                };

            if (order.Customer == null || order.Customer.UserId != userId)
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

            var existingPayment = await _paymentRepository.GetByOrderIdAsync(order.Id);
            if (existingPayment != null)
            {
                return new OrderPaymentResponse
                {
                    Success = false,
                    Message = "This order is already paid."
                };
            }

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
                    Status = Status.Active
                };

                await _paymentRepository.AddAsync(cashPayment);
                await _paymentRepository.SaveAsync();

                order.OrderStatusEnum = OrderStatus.Completed;
                await _orderRepository.UpdateAsync(order);
                await _orderRepository.SaveAsync();

                return new OrderPaymentResponse
                {
                    Success = true,
                    Message = "Cash payment completed successfully.",
                    Url = null,
                    PaymentId = null,
                    Amount = totalAmount
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
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            return new OrderPaymentResponse
            {
                Success = true,
                Message = "Payment processed successfully.",
                Url = session.Url,
                PaymentId = session.Id,
                Amount = totalAmount
            };
        }

        public async Task<bool> HandleVisaPaymentSuccessAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
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

            var existingPayment = await _paymentRepository.GetByOrderIdAsync(order.Id);
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
                Status = Status.Active
            };

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveAsync();

            order.OrderStatusEnum = OrderStatus.Completed;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveAsync();

            return true;
        }
    }
}
