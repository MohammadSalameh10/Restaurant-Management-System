namespace RestaurantOps.DAL.DTO.Responses
{
    public class SupplierResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int LocationId { get; set; }
        public string LocationCity { get; set; }
        public string LocationStreet { get; set; }
    }
}
