using Microsoft.AspNetCore.Identity;

namespace RestaurantOps.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? city { get; set; }
        public string? street { get; set; }
        public string? CodeResetPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }

    }
}
