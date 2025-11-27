using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.DTO.Responses
{
    public class OrderPaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Url { get; set; }
    }
}
