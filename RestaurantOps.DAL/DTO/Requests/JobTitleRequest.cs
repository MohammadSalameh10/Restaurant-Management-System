namespace RestaurantOps.DAL.DTO.Requests
{
    public class JobTitleRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PayRate { get; set; }
    }
}
