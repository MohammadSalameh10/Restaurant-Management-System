using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Interfaces;
using CustomerModel = RestaurantOps.DAL.Models.Customer;

namespace RestaurantOps.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly ICustomerRepository _customerRepository;

        public OrdersController(
            IOrderService orderService,
            IPaymentService paymentService,
            ICustomerRepository customerRepository)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CustomerOrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            
            var customer = await _customerRepository.GetByUserIdAsync(userId);

          
            if (customer == null)
            {
                customer = new CustomerModel
                {
                    Name = User.Identity?.Name ?? "Customer",
                    PhoneNumber = "",
                    UserId = userId,
                    LocationId = 1,
                    CreatedAt = DateTime.UtcNow,
                    Status = Status.Active
                };

                await _customerRepository.AddAsync(customer);
                await _customerRepository.SaveAsync();
            }

           
            var orderRequest = new OrderCreateRequest
            {
                CustomerId = customer.Id,
                EmployeeId = null,
                OrderTypeId = request.OrderTypeId,
                Items = request.Items.Select(i => new OrderItemCreateRequest
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var orderId = await _orderService.CreateOrderAsync(orderRequest);

            if (orderId == 0)
                return BadRequest("Insufficient inventory for one or more items.");

            return Ok(new { OrderId = orderId });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{id}/pay")]
        public async Task<ActionResult> PayOrder(int id, [FromBody] PayOrderRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var method = string.IsNullOrWhiteSpace(model?.Method)
                ? "Cash"
                : model.Method.Trim();

            var request = new OrderPaymentRequest
            {
                OrderId = id,
                Method = method
            };

            var response = await _paymentService.ProcessOrderPaymentAsync(request, userId, Request);

            if (!response.Success)
                return BadRequest(new { response.Message });

            return Ok(response);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<OrderResponse>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var orders = await _orderService.GetCustomerOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<ActionResult> CancelMyOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _orderService.CancelOrderForCustomerAsync(id, userId);
            if (!success)
                return BadRequest("Unable to cancel this order.");

            return Ok("Order canceled successfully.");
        }
    }
}
