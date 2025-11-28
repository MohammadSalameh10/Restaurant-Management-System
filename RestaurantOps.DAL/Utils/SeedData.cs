using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;

namespace RestaurantOps.DAL.Utils
{
    public class SeedData : ISeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedData(
           ApplicationDbContext context,
           RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager
           )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task IdentityDataSeedingAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser()
                {
                    Email = "mohammad@gmail.com",
                    FullName = "Mohammad Salameh",
                    PhoneNumber = "0592100105",
                    UserName = "Msalameh",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser()
                {
                    Email = "sleman@gmail.com",
                    FullName = "Sleman Hmidat",
                    PhoneNumber = "0592100104",
                    UserName = "Shmidat",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser()
                {
                    Email = "Amr@gmail.com",
                    FullName = "Amr Foqha",
                    PhoneNumber = "0592100102",
                    UserName = "Afoqha",
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user1, "Pass@123123");
                await _userManager.CreateAsync(user2, "Pass@123123");
                await _userManager.CreateAsync(user3, "Pass@123123");

                await _userManager.AddToRoleAsync(user1, "Admin");
                await _userManager.AddToRoleAsync(user2, "Customer");
                await _userManager.AddToRoleAsync(user3, "Customer");

            }

            await _context.SaveChangesAsync();
        }
    }
}
