using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.Models
{
    public class OrderItem : BaseModel
    {
        public int Quantity { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
