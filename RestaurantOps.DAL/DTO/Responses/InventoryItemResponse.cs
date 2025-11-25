namespace RestaurantOps.DAL.DTO.Responses
{
    public class InventoryItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
