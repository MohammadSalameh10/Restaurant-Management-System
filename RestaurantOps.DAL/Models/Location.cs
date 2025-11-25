namespace RestaurantOps.DAL.Models
{
    public class Location : BaseModel
    {
        public string City { get; set; }
        public string Street { get; set; }
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
