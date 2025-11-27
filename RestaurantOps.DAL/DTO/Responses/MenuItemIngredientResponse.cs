using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOps.DAL.DTO.Responses
{
    public class MenuItemIngredientResponse
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }

        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }

        public int InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }
    }
}
