using Inventory.BLL.DTOs.User;
using Inventory.BLL.Interface;
using InventoryApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
         private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
         

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user, "User retrieved successfully"));
        }


        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user, "User retrieved successfully"));
        }


        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = _userService.GetLoggedInUser();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<string>.FailResponse("User not logged in"));

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(ApiResponse<string>.FailResponse("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user, "Current user retrieved successfully"));
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassDto dto)
        {
            dto.userId = _userService.GetLoggedInUser();
            var result = await _userService.ChangePasswordAsync(dto);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password changed successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse(result.Errors.ToList(), "Failed to change password"));
        }



    }
}
