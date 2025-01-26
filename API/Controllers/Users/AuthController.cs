using Microsoft.AspNetCore.Mvc;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;

namespace WealthFlow.API.Controllers.Users
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO registerUserDTO)
        {
            if (registerUserDTO == null)
                return BadRequest("Registration Failed.Please Try again");

            var result = await _authService.RegisterAsync(registerUserDTO);

            return StatusCode(result.StatusCode, new { message = result.ErrorMessage ?? "User Registered successfully." });
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDTO>> Login(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
                return BadRequest("Credentials are missing. Please try again.");

            var token = await _authService.LoginAsync(userLoginDTO);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Failed Credentials. Try again with correct credentials.");

            return Ok(new { Token = token });
        }
    }
}
