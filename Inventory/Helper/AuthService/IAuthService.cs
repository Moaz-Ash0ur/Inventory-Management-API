using Inventory.BLL.DTOs.User;
using System.Security.Claims;
using static InventoryApi.Helper.AuthService;

namespace InventoryApi.Helper
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto request);
        Task<AuthResult> LoginAsync(LoginDto request);
        Task<AuthResult> GoogleLogin(TokenRequestDto request);
        Task<AuthResult> RefreshAccessTokenAsync(string refreshToken);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();

    }





}
