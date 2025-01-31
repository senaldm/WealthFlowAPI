using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.API.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    [Authorize]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        //get logged user
        //[HttpGet("me")]
        //public async Task<ActionResult<UserDTO>> GetCurrentUser(string token)
        //{
        //    var id =await _authService.IsUserAuthenticated(token);

        //    if (id == null)
        //        return Unauthorized();

        //    Guid userId = id.Value;
        //    var user = await _userService.GetUserByIdAsync(userId);
        //    if (user == null)
        //        return NotFound("User Not Found");

        //    return Ok(user);

        //}


        //update logged user details
        [HttpPut("me")]
        public async Task<Result<Object>> UpdateCurrentUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            return await _userService.UpdateUserAsync( updateUserDTO);
        }

        //get all users(Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<Result<Object>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();

        }


        [HttpDelete("me")]
        public async Task<Result<string>> DeleteCurrentUser()
        {
            return await _userService.DeleteUserAsync();
        }


        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<string>> DeleteUserById()
        {
            return await _userService.DeleteUserAsync();
        }
    }
}
