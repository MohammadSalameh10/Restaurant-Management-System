using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantOps.BLL.Services.Interfaces;
using RestaurantOps.DAL.DTO.Requests;
using RestaurantOps.DAL.DTO.Responses;
using RestaurantOps.DAL.Models;
using RestaurantOps.DAL.Repositories.Classes;
using RestaurantOps.DAL.Repositories.Interfaces;

namespace RestaurantOps.BLL.Services.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILocationRepository _locationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJobTitleRepository _jobTitleRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailSender emailSender, SignInManager<ApplicationUser> signInManager,
            ILocationRepository locationRepository,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IJobTitleRepository jobTitleRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _locationRepository = locationRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _jobTitleRepository = jobTitleRepository;
            _roleManager = roleManager;
        }
        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                throw new Exception("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
            if (result.Succeeded)
            {
                return new UserResponse()
                {
                    Token = await CreateTokenAsync(user)
                };
            }
            else if (result.IsLockedOut)
            {
                throw new Exception("Your Account is locked.");
            }
            else if (result.IsNotAllowed)
            {
                throw new Exception("You are not allowed to login. Please confirm your email.");
            }
            else
            {
                throw new Exception("Invalid email or password");
            }

        }

        public async Task<string> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "Email confirmed successfully";
            }
            return "Email confirmation failed";
        }
        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest, HttpRequest request)
        {
            var user = new ApplicationUser()
            {
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                UserName = registerRequest.UserName
            };

            var Result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (Result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var escapeToken = Uri.EscapeDataString(token);
                var emailUrl = $"{request.Scheme}://{request.Host}/api/identity/Accounts/ConfirmEmail?token={escapeToken}&userId={user.Id}";

                await _userManager.AddToRoleAsync(user, "Customer");

                var locations = await _locationRepository.GetAllAsync();
                var defaultLocationId = locations.FirstOrDefault()?.Id ?? 1;

                var customer = new Customer
                {
                    UserId = user.Id,
                    Name = user.FullName ?? user.UserName ?? "Customer",
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    LocationId = defaultLocationId,
                    CreatedAt = DateTime.UtcNow,
                    Status = Status.Active
                };

                await _customerRepository.AddAsync(customer);
                await _customerRepository.SaveAsync();

                await _emailSender.SendEmailAsync(user.Email, "Welcome", $"<h1>Hello {user.UserName}</h1>" +
                    $"<a href='{emailUrl}'> confirm </a>");
                return new UserResponse()
                {
                    Token = registerRequest.Email
                };
            }
            else
            {
                throw new Exception($"{Result.Errors}");
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwtOptions")["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                $"<h1>Hello {user.UserName}</h1>" +
                $"<p>Your reset password code is: {code}</p>" +
                $"<p>It will expire in 15 minutes.</p>");

            return true;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.CodeResetPassword != request.Code ||
                user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                return false;

            user.CodeResetPassword = null;
            user.PasswordResetCodeExpiry = null;
            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(user.Email, "Password Reset Successful",
                $"<h1>Hello {user.UserName}</h1>" +
                $"<p>Your password has been reset successfully.</p>");

            return true;
        }

        public async Task<bool> ChangeUserRoleAsync(string userId, ChangeUserRoleRequest model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();

            if (string.Equals(currentRole, model.NewRole, StringComparison.OrdinalIgnoreCase))
                return true; 

      
            if (string.Equals(currentRole, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                var customer = await _customerRepository.GetByUserIdAsync(user.Id);
                if (customer != null)
                {
                    customer.Status = Status.Inactive;
                    await _customerRepository.UpdateAsync(customer);
                    await _customerRepository.SaveAsync();
                }
            }

            if (string.Equals(currentRole, "Employee", StringComparison.OrdinalIgnoreCase))
            {
                var employee = await _employeeRepository.GetByUserIdAsync(user.Id);
                if (employee != null)
                {
                    employee.Status = Status.Inactive;
                    await _employeeRepository.UpdateAsync(employee);
                    await _employeeRepository.SaveAsync();
                }
            }

            if (string.Equals(model.NewRole, "Customer", StringComparison.OrdinalIgnoreCase))
            {
                var existingCustomer = await _customerRepository.GetByUserIdAsync(user.Id);
                if (existingCustomer == null)
                {
                    var locationId = model.LocationId;

                    if (locationId == null)
                    {
                        var locations = await _locationRepository.GetAllAsync();
                        locationId = locations.FirstOrDefault()?.Id ?? 1;
                    }

                    var customer = new Customer
                    {
                        UserId = user.Id,
                        Name = user.FullName ?? user.UserName ?? "Customer",
                        PhoneNumber = user.PhoneNumber ?? string.Empty,
                        LocationId = locationId.Value,
                        CreatedAt = DateTime.UtcNow,
                        Status = Status.Active
                    };

                    await _customerRepository.AddAsync(customer);
                    await _customerRepository.SaveAsync();
                }
            }
            else if (string.Equals(model.NewRole, "Employee", StringComparison.OrdinalIgnoreCase))
            {
                var existingEmployee = await _employeeRepository.GetByUserIdAsync(user.Id);
                if (existingEmployee == null)
                {
                    var jobTitleId = model.JobTitleId;

                    if (jobTitleId == null)
                    {
                        var jobTitles = await _jobTitleRepository.GetAllAsync();
                        jobTitleId = jobTitles.FirstOrDefault()?.Id ?? 1;
                    }

                    var employee = new Employee
                    {
                        UserId = user.Id,
                        Name = user.FullName ?? user.UserName ?? "Employee",
                        DateOfBirth = new DateTime(2000, 1, 1), 
                        JobTitleId = jobTitleId.Value,
                        CreatedAt = DateTime.UtcNow,
                        Status = Status.Active
                    };

                    await _employeeRepository.AddAsync(employee);
                    await _employeeRepository.SaveAsync();
                }
            }

          
            if (!string.IsNullOrEmpty(currentRole))
                await _userManager.RemoveFromRoleAsync(user, currentRole);

            if (!await _roleManager.RoleExistsAsync(model.NewRole))
                return false;

            await _userManager.AddToRoleAsync(user, model.NewRole);

            return true;
        }

        public async Task<List<UserListResponse>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            var list = new List<UserListResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                list.Add(new UserListResponse
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "None"
                });
            }

            return list;
        }
    }
}
