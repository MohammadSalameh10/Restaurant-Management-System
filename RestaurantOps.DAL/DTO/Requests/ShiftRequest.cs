namespace RestaurantOps.DAL.DTO.Requests
{
    public class ShiftRequest
    {
        public int EmployeeId { get; set; }
        public DateTime ExpectedCheckIn { get; set; }
        public DateTime ExpectedCheckOut { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool HasWorked { get; set; }
    }
}
