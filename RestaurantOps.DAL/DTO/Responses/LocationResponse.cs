using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.DTO.Responses
{
    public class LocationResponse
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
