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
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task DataSeedingAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                await _context.Database.MigrateAsync();

            var now = DateTime.UtcNow;

            if (!await _context.Locations.AnyAsync())
            {
                await _context.Locations.AddRangeAsync(
                    new Location { City = "Ramallah", Street = "Al-Tireh", CreatedAt = now, Status = Status.Active },
                    new Location { City = "Nablus", Street = "Downtown", CreatedAt = now, Status = Status.Active },
                    new Location { City = "Hebron", Street = "City Center", CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }

            var defaultLocation = await _context.Locations.FirstAsync();

            if (!await _context.JobTitles.AnyAsync())
            {
                await _context.JobTitles.AddRangeAsync(
                    new JobTitle { Name = "Chef", Description = "Main kitchen chef", PayRate = 25, CreatedAt = now, Status = Status.Active },
                    new JobTitle { Name = "Waiter", Description = "Serves customers", PayRate = 15, CreatedAt = now, Status = Status.Active },
                    new JobTitle { Name = "Cashier", Description = "Handles payments", PayRate = 18, CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }

            if (!await _context.Suppliers.AnyAsync())
            {
                await _context.Suppliers.AddRangeAsync(
                    new Supplier { Name = "Fresh Foods Supplier", PhoneNumber = "0599000000", LocationId = defaultLocation.Id, CreatedAt = now, Status = Status.Active },
                    new Supplier { Name = "Cold Drinks Supplier", PhoneNumber = "0599111111", LocationId = defaultLocation.Id, CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }

            var suppliers = await _context.Suppliers.ToListAsync();
            var mainSupplier = suppliers.First();

            if (!await _context.InventoryItems.AnyAsync())
            {
                await _context.InventoryItems.AddRangeAsync(
                    new InventoryItem { Name = "Flour", Stock = 100, SupplierId = mainSupplier.Id, CreatedAt = now, Status = Status.Active },
                    new InventoryItem { Name = "Cheese", Stock = 50, SupplierId = mainSupplier.Id, CreatedAt = now, Status = Status.Active },
                    new InventoryItem { Name = "Beef", Stock = 40, SupplierId = mainSupplier.Id, CreatedAt = now, Status = Status.Active },
                    new InventoryItem { Name = "Cola", Stock = 80, SupplierId = mainSupplier.Id, CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }

            var inventoryItems = await _context.InventoryItems.ToListAsync();

            if (!await _context.MenuItems.AnyAsync())
            {
                await _context.MenuItems.AddRangeAsync(
                    new MenuItem { ItemName = "Cheese Burger", Description = "Burger with beef and cheese", IsAvailable = true, Price = 25, CreatedAt = now, Status = Status.Active },
                    new MenuItem { ItemName = "Margherita Pizza", Description = "Classic pizza with cheese", IsAvailable = true, Price = 30, CreatedAt = now, Status = Status.Active },
                    new MenuItem { ItemName = "Cola Drink", Description = "Cold soft drink", IsAvailable = true, Price = 5, CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }

            var menuItems = await _context.MenuItems.ToListAsync();

            if (!await _context.MenuItemIngredients.AnyAsync())
            {
                var flour = inventoryItems.FirstOrDefault(i => i.Name == "Flour");
                var cheese = inventoryItems.FirstOrDefault(i => i.Name == "Cheese");
                var meat = inventoryItems.FirstOrDefault(i => i.Name == "Beef");
                var cola = inventoryItems.FirstOrDefault(i => i.Name == "Cola");

                var burger = menuItems.FirstOrDefault(m => m.ItemName == "Cheese Burger");
                var pizza = menuItems.FirstOrDefault(m => m.ItemName == "Margherita Pizza");
                var drink = menuItems.FirstOrDefault(m => m.ItemName == "Cola Drink");

                var list = new List<MenuItemIngredient>();

                if (burger != null && meat != null)
                    list.Add(new MenuItemIngredient { MenuItemId = burger.Id, InventoryItemId = meat.Id, Quantity = 0.2m, CreatedAt = now, Status = Status.Active });

                if (burger != null && cheese != null)
                    list.Add(new MenuItemIngredient { MenuItemId = burger.Id, InventoryItemId = cheese.Id, Quantity = 0.05m, CreatedAt = now, Status = Status.Active });

                if (pizza != null && flour != null)
                    list.Add(new MenuItemIngredient { MenuItemId = pizza.Id, InventoryItemId = flour.Id, Quantity = 0.3m, CreatedAt = now, Status = Status.Active });

                if (pizza != null && cheese != null)
                    list.Add(new MenuItemIngredient { MenuItemId = pizza.Id, InventoryItemId = cheese.Id, Quantity = 0.08m, CreatedAt = now, Status = Status.Active });

                if (drink != null && cola != null)
                    list.Add(new MenuItemIngredient { MenuItemId = drink.Id, InventoryItemId = cola.Id, Quantity = 1m, CreatedAt = now, Status = Status.Active });

                if (list.Any())
                {
                    await _context.MenuItemIngredients.AddRangeAsync(list);
                    await _context.SaveChangesAsync();
                }
            }

            if (!await _context.OrderTypes.AnyAsync())
            {
                await _context.OrderTypes.AddRangeAsync(
                    new OrderType { Name = "Dine In", CreatedAt = now, Status = Status.Active },
                    new OrderType { Name = "Take Away", CreatedAt = now, Status = Status.Active },
                    new OrderType { Name = "Delivery", CreatedAt = now, Status = Status.Active }
                );
                await _context.SaveChangesAsync();
            }
        }

        public async Task IdentityDataSeedingAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    Email = "mohammad@gmail.com",
                    FullName = "Mohammad Salameh",
                    PhoneNumber = "0592100105",
                    UserName = "Msalameh",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    Email = "sleman@gmail.com",
                    FullName = "Sleman Hmidat",
                    PhoneNumber = "0592100104",
                    UserName = "Shmidat",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
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
                await _userManager.AddToRoleAsync(user3, "Employee");

                _context.Customers.Add(new Customer
                {
                    UserId = user2.Id,
                    Name = user2.FullName,
                    PhoneNumber = user2.PhoneNumber,
                    LocationId = 1,
                    CreatedAt = DateTime.UtcNow,
                    Status = Status.Active
                });

                _context.Employees.Add(new Employee
                {
                    UserId = user3.Id,
                    Name = user3.FullName,
                    DateOfBirth = new DateTime(1998, 1, 1),
                    JobTitleId = 1,
                    CreatedAt = DateTime.UtcNow,
                    Status = Status.Active
                });
            }
            

            await _context.SaveChangesAsync();
        }
    }
}
