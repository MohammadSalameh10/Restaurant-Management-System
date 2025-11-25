namespace RestaurantOps.DAL.DTO.Responses
{
    public class MenuItemResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}
