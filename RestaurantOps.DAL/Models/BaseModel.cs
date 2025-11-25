namespace RestaurantOps.DAL.Models
{
    public enum Status
    {
        Active = 1,
        Inactive = 2,
    }
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status status { get; set; }
    }
}
