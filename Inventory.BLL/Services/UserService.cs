using AutoMapper;
using Inventory.BLL.DTOs.User;
using Inventory.BLL.Interface;
using Inventory.DAL.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory.BLL.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

      

        public async Task<UserResultDto> RegisterAsync(RegisterDto dto)
        {

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,LastName = dto.LastName,
                UserName = dto.Email, Email = dto.Email,
                PhoneNumber = dto.Phone
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return FailResult(createResult.Errors);

            var roleName = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role;
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
                return FailResult(roleResult.Errors);

          //  await SendEmailConfirmationAsync(user);

             return new UserResultDto { Success = true }; ;
        }

        #region Helpers
        private UserResultDto FailResult(IEnumerable<IdentityError> errors)
        {
            return new UserResultDto
            {
                Success = false,
                Errors = errors.Select(e => e.Description)
            };
        }

        #endregion



        public async Task<UserResultDto> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return new UserResultDto { Success = false, Errors = new[] { "Invalid confirmation data" } };

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            token = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Email Confirmation Failed" } };
        }

        public async Task<UserResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new UserResultDto { Success = false, Errors = new[] { "Invalid email or password" } };

            //if (!user.EmailConfirmed)
            //    return new UserResultDto { Success = false, Errors = new[] { "Please confirm your email first." } };

            return new UserResultDto { Success = true};
        }

        public async Task<UserResultDto> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://localhost:7003/api/auth/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

          
            return new UserResultDto { Success = true };
        }

        public async Task<UserResultDto> ResetPasswordAsync(ResetPassDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.email);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var result = await _userManager.ResetPasswordAsync(user, dto.token, dto.newPassword);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Failed to reset password" } };
        }

        public async Task<UserResultDto> ChangePasswordAsync(ChangePassDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userId);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var result = await _userManager.ChangePasswordAsync(user, dto.currentPassword, dto.newPassword);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Failed to change password" } };
        }

    
        public string GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId!;
        }

     
        //User Info

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
           IEnumerable<ApplicationUser> result = await _userManager.Users.ToListAsync();

           var users =  _mapper.Map<IEnumerable<UserDto>>(result);

            return users;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }


        //For Authn
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Role = roles.FirstOrDefault()!
            };
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Role = roles.FirstOrDefault()!
            };
        }

        public async Task<(Claim[], UserDto)> GetClaims(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return (claims, user);
        }

        public async Task<Claim[]> GetClaimsById(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            return claims;
        }



    }


}


