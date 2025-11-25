namespace RestaurantOps.DAL.DTO.Requests
{
    public class MenuItemRequest
    {
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
