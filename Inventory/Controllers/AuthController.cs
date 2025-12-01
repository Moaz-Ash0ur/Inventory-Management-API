using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.BLL.DTOs.User;
using Inventory.BLL.Interface;
using InventoryApi.Helper;
using static InventoryApi.Helper.AuthService;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IAuthService _authService;


        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }



        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">User registration details (email, password, fullname...)</param>
        /// <returns>ApiResponse with user details</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result == null)
                return BadRequest(ApiResponse<AuthResult>.FailResponse("Failed to register user"));

            return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "User registered successfully. Please check your email."));
        }


        /// <summary>
        /// Login with email and password to get access & refresh tokens.
        /// </summary>
        /// <param name="request">Login DTO (email + password)</param>
        /// <returns>ApiResponse with JWT tokens</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.FailResponse(result.Errors.ToList(), "Invalid login attempt"));


            SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);


            return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Login successful"));
        }
        //search more
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] TokenRequestDto request)
        {
            var result = await _authService.GoogleLogin(request);

            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.FailResponse(result.Errors.ToList(), "Invalid login attempt"));

            return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Login successful"));
        }



        /// <summary>
        /// Refresh access token using a valid refresh token.
        /// </summary>
        /// <returns>ApiResponse with new JWT tokens</returns>
        [HttpPost("refreshAccessToken")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> RefreshAccessToken()
        {
            var refreshToken = GetRefreshTokenCookie();

            var result = await _authService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.FailResponse(result.Errors.ToList(), "Failed to refresh token"));


            return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Token refreshed successfully"));
        }



        //Set and Get Token in Client-Browser Http only Cookie
        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookies = new CookieOptions
            {
                HttpOnly = true,
                //   Secure = true,   
                // SameSite = SameSiteMode.Strict, 
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookies);
        }


        private string GetRefreshTokenCookie()
        {
            return Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) ? refreshToken : string.Empty;
        }


        [HttpGet("confirm-email", Name = "confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Email confirmed successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse(result.Errors.ToList(), "Email confirmation failed"));
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPasswordAsync(email);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password reset link sent to your email"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse(result.Errors.ToList(), "Failed to send reset link"));
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password reset successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse(result.Errors.ToList(), "Failed to reset password"));
        }



       


    }
}
