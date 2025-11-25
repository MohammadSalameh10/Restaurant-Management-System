namespace RestaurantOps.DAL.Models
{
    public class Supplier : BaseModel
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}
