namespace RestaurantOps.DAL.DTO.Requests
{
    public class ChangeUserRoleRequest
    {
        public string NewRole { get; set; } = null!;
        public int? JobTitleId { get; set; }     
        public int? LocationId { get; set; }
    }
}
