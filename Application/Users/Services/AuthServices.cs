using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Shared.Helpers;
using WealthFlow.Infrastructure.ExternalServices.MailServices;
using System.Net;

namespace WealthFlow.Application.Users.Services
{
    public class AuthServices: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthServices(IAuthRepository authRepository, IUserRepository userRepository, 
            IPasswordService passwordService, ITokenService jwtTokenService, 
            IUserService userService, IConfiguration configuration,
            IEmailService emailService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = jwtTokenService;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<Result>  RegisterAsync(UserRegistrationDTO registerUserDTO)
        {
            bool isUnique = await _userRepository.IsEmailUniqueAsync(registerUserDTO.Email);

            if (!isUnique)
                return Result.Failure("Email is already in use.", HttpStatusCode.Conflict);

            string hashedPassword = _passwordService.HashPassword(registerUserDTO.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerUserDTO.Name,
                Email = registerUserDTO.Email,
                Password = hashedPassword,
                PhoneNumber = string.IsNullOrEmpty(registerUserDTO.PhoneNumber) ? null : registerUserDTO.PhoneNumber,
                RecoveryEmail = string.IsNullOrEmpty(registerUserDTO.RecoveryEmail) ? null : registerUserDTO.RecoveryEmail
            };

            bool isCreated =  await _authRepository.CreateUserAsync(user);
            if (!isCreated)
                return Result.Failure("Failed to create user due to internal error!", HttpStatusCode.InternalServerError);

            return Result.Success("User Registers Successfully! ", HttpStatusCode.Created);
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return Result.Failure("Invalid Email. Please try again", HttpStatusCode.Unauthorized);

            if (!_passwordService.VerifyPassword(password, user.Password))
                return Result.Failure("Invalid password.Plese try again.", HttpStatusCode.Unauthorized);

            var userDTO = _userService.extractUserDTOFromUser(user);

            return await _tokenService.GenerateJwtToken(userDTO);
        }
       
        public async Task<Result> ChangePasswordAsync(string token, string newPassword)
        {
            var userId = await _tokenService.GetUserIdFromJwtTokenIfValidated(token);
            if (userId == null)
                return Result.Failure("Unorthorized Request, Please login again.", HttpStatusCode.Unauthorized);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);
            if (user == null)
                return Result.Failure("User not found", HttpStatusCode.BadRequest);

            return await _passwordService.UpdatePasswordIfValidatedAsync(user, newPassword);
        }

        public async Task<Result> RequestToResetPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return Result.Failure("No user registerd with this email.", HttpStatusCode.Unauthorized);

            string verificationToken = _passwordService.GeneratePasswordResetToken();
            bool isSaved = await _tokenService.StorePasswordResetToken(user.Id, verificationToken);

            if (!isSaved)
                return Result.Failure("Couldn't to complete the request. Try again", HttpStatusCode.InternalServerError);

            var verificationLink = GenerateVerificationLink(verificationToken);

            return await _emailService.SendPasswordResetLinkAsync(user.Email, verificationLink);
        }

        private string GenerateVerificationLink(string token)
        {
            var baseUrl = _configuration["AppSettings:FrontendUrl"];
            //var verificationKey = $"password_reset:{key}";

            return $"{baseUrl}/reset-password?token={token}";
        }

        public async Task<Result> ForgotEmail(string recoveryEmail)
        {
            var email = await _userRepository.GetUserEmailUsingRecoveryEmail(recoveryEmail);
            if (email == null)
                return Result.Failure("No any emails matched to entered email. Try to add correct recovery email", HttpStatusCode.Unauthorized);

            return Result.Success(email, HttpStatusCode.OK);
        }

        public async Task<Result> ResetPassword(string key, string newPassword)
        {

            string userId = await _tokenService.GetPasswordResetTokenIfAny(key);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure("Invalid or Exprired password reset link", HttpStatusCode.BadRequest);

            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));

            if (user == null)
                return Result.Failure("User not found", HttpStatusCode.BadRequest);

            return await _passwordService.UpdatePasswordIfValidatedAsync(user, newPassword);

        }

        private Guid GetUserIdFromJwtToken(string JwtToken)
        {
            string userId = JwtToken.Replace("jwt-token:", "");

            return Guid.Parse(userId);
        }

        private  bool GetExistUser(Guid userId)
        {
            var user =  _userRepository.GetUserByIdAsync(userId);

           if(user == null)
                return false;
            return true;
        }

    }
}
