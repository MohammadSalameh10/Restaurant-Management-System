using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
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

        public OrdersController(IOrderService orderService, IPaymentService paymentService, ICustomerRepository customerRepository)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public ActionResult CreateOrder([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customer = _customerRepository.GetByUserId(userId);

            if (customer == null)
            {
                customer = new CustomerModel
                {
                    Name = "Customer",
                    PhoneNumber = "",
                    UserId = userId
                };

                _customerRepository.Add(customer);
                _customerRepository.Save();
            }

            request.CustomerId = customer.Id;

            var orderId = _orderService.CreateOrder(request);

            if (orderId == 0)
                return BadRequest("Insufficient inventory for one or more items.");

            return Ok(new { OrderId = orderId });
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> GetById(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{id}/pay")]
        public async Task<ActionResult> PayOrder(int id, [FromBody] string method)
        {
            if (string.IsNullOrWhiteSpace(method))
                method = "Cash";

            var request = new OrderPaymentRequest
            {
                OrderId = id,
                Method = method
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _paymentService.ProcessOrderPaymentAsync(request, userId, Request);
            if (!response.Success)
                return BadRequest(new { response.Message });

            return Ok(response);
        }

        [HttpGet("my")]
        public ActionResult<List<OrderResponse>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var orders = _orderService.GetCustomerOrders(userId);
            return Ok(orders);
        }

        [HttpPatch("{id}/cancel")]
        public ActionResult CancelMyOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = _orderService.CancelOrderForCustomer(id, userId);
            if (!success)
                return BadRequest("Unable to cancel this order.");

            return Ok("Order canceled successfully.");
        }

    }
}
