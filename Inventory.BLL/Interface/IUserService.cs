using Inventory.BLL.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;

namespace Inventory.BLL.Interface
{
    public interface IUserService
    {
        Task<UserResultDto> RegisterAsync(RegisterDto dto);
        Task<UserResultDto> ConfirmEmailAsync(string userId, string token);
        Task<UserResultDto> LoginAsync(LoginDto dto);
        Task<UserResultDto> ForgotPasswordAsync(string email);
        Task<UserResultDto> ResetPasswordAsync(ResetPassDto dto);
        Task<UserResultDto> ChangePasswordAsync(ChangePassDto dto);
        string GetLoggedInUser();


        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> GetUserProfileAsync(string userId);

        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<(Claim[], UserDto)> GetClaims(string email);
        Task<Claim[]> GetClaimsById(string userId);
        



    }


}


