namespace RestaurantOps.DAL.DTO.Responses
{
    public class JobTitleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PayRate { get; set; }
    }
}
