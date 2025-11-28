using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.DTO.Responses
{
    public class TopMenuItemResponse
    {
        public string ItemName { get; set; }
        public int QuantitySold { get; set; }
    }
}
