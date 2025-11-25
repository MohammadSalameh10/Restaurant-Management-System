namespace RestaurantOps.DAL.Models
{
    public class JobTitle : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PayRate { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
