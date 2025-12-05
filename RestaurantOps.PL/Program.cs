using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using RestaurantOps.BLL.MapsterConfigurations;
using RestaurantOps.BLL.Services.Classes;
using RestaurantOps.DAL.Data;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Utils;
using Scalar.AspNetCore;
using Stripe;

namespace RestaurantOps.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            var keysPath = Path.Combine(builder.Environment.ContentRootPath, "keys");
            Directory.CreateDirectory(keysPath);

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
                .SetApplicationName("RestaurantOps");

            builder.Services.AddConfig();
            builder.Services.MapsterConfigRegister();

            var jwtKey = builder.Configuration["jwtOptions:SecretKey"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("jwtOptions:SecretKey is missing in appsettings.json");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            var stripeSecret = builder.Configuration["Stripe:SecretKey"];
            if (!string.IsNullOrWhiteSpace(stripeSecret))
            {
                builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
                StripeConfiguration.ApiKey = stripeSecret;
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<ISeedData>();
                    await seeder.DataSeedingAsync();
                    await seeder.IdentityDataSeedingAsync();

                    logger.LogInformation("DATABASE SEEDED SUCCESSFULLY");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error during database seeding");
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

