using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using Microsoft.IdentityModel.Tokens;


namespace WealthFlow.API.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    [Authorize]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        //get logged user
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound("User Not Found");

            return Ok(user);

        }


        //update logged user details
        [HttpPut("me")]
        public async Task<ActionResult<UpdateUserDTO>> UpdateCurrentUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _userService.UpdateUserAsync(Guid.Parse(userId), updateUserDTO);
            if (!result)
                return BadRequest("Failed to update userData");

            return Ok("Profile Update SuccessFully");
        }


        //get all users(Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }


        [HttpDelete("me")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            var userId = GetCurrentUserId();
            if(string.IsNullOrEmpty(userId))
                return NotFound();

            bool isDeleted = await _userService.DeleteUserAsync(Guid.Parse(userId));

            if (!isDeleted)
                return BadRequest("Account Destroy Failed.Try Again !");

            return Ok("Account Destroy Successfully?");
        }


        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            bool isDeleted = await _userService.DeleteUserAsync(Guid.Parse(userId));

            if(!isDeleted)
                return BadRequest("Account Destroy Failed.Try Again !");

            return Ok("Account Desrtroy Successfully!");
        }
    }
}
