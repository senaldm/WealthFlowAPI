using Microsoft.AspNetCore.Mvc;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Shared.Helpers;
using WealthFlow.Shared.Helpers.Enums;

namespace WealthFlow.API.Controllers.Users
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<Result> RegisterUser([FromBody] UserRegistrationDTO registerUserDTO)
        {
            if (registerUserDTO == null)
                return Result.Failure("You must fill the fields");

            if (string.IsNullOrEmpty(registerUserDTO.Name))
                return Result.Failure("Name is required");


            if (string.IsNullOrEmpty(registerUserDTO.Email))
                return Result.Failure("Email is required");

            if (string.IsNullOrEmpty(registerUserDTO.Password))
                return Result.Failure("Password is required");

            return await _authService.RegisterAsync(registerUserDTO);

        }


        [HttpPost("login")]
        public async Task<Result> Login([FromBody]UserLoginDTO loginForm)
        {
            if (string.IsNullOrEmpty(loginForm.Email))
                return Result.Failure("Email is required.");

            if (string.IsNullOrEmpty(loginForm.Password))
                return Result.Failure("Password is required.");

            return await _authService.LoginAsync(loginForm.Email, loginForm.Password);
        }

        [HttpPost("change-password")]
        public async Task<Result> ChangePassword(string newPassword)
        {

            return await _authService.ChangePasswordAsync(newPassword);
        }

        [HttpPost("password-reset-request")]
        public async Task<Result> RequestToResetPasswordAsync(string email)
        {
            return await _authService.RequestToResetPasswordAsync(email);
        }

        [HttpPost("forgot-email")]
        public async Task<Result> ForgotEmailAsync(string recoveryEmail)
        {
            return await _authService.ForgotEmail(recoveryEmail);
        }

        [HttpPost("refresh-jwt-token")]
        public async Task<Result> RefreshJwtTokenAsync(string jwtKey)
        {
            return await _tokenService.RefreshTokenAsync(jwtKey);
        }

        [HttpPost("password-reset")]
        public async Task<Result> ResetPassword(string key, string newPassword)
        {
            return await _authService.ResetPassword(key, newPassword);
        }


    }
}
