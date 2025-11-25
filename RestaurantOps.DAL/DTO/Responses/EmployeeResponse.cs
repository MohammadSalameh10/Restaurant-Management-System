namespace RestaurantOps.DAL.DTO.Responses
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public decimal JobTitlePayRate { get; set; }
    }
}
