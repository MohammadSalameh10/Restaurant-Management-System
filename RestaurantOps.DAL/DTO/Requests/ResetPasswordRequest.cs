namespace RestaurantOps.DAL.DTO.Requests
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
