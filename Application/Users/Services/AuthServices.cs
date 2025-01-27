using System.Globalization;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Infrastructure.Caching;
using WealthFlow.Shared.Helpers;
using WealthFlow.Infrastructure.ExternalServices.MailServices;
using WealthFlow.Application.Caching.Interfaces;
using static WealthFlow.Domain.Enums.Enum;
using static WealthFlow.Shared.Helpers.Enums.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Components.Web;

namespace WealthFlow.Application.Users.Services
{
    public class AuthServices: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ICacheService _cacheService;

        public AuthServices(IAuthRepository authRepository, IUserRepository userRepository, 
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, 
            IUserService userService, IConfiguration configuration,
            IEmailService emailService, ICacheService cacheService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
            _cacheService = cacheService;
        }

        public async Task<Result>  RegisterAsync(UserRegistrationDTO registerUserDTO)
        {
            bool isUnique = await _userRepository.IsEmailUniqueAsync(registerUserDTO.Email);

            if (!isUnique)
                return Result.Failure("Email is already in use.", (int)StatusCode.CONFLICT);

            string hashedPassword = _passwordHasher.HashPassword(registerUserDTO.Password);

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
                return Result.Failure("Failed to create user due to internal error!", (int)StatusCode.INTERNAL_SERVER_ERROR);

            return Result.Success("User Registers Successfully! ", (int)StatusCode.CREATED);
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return Result.Failure("Invalid Email. Please try again", (int)StatusCode.UNAUTHORIZED);

            if (!_passwordHasher.VerifyPassword(password, user.Password))
                return Result.Failure("Invalid password.Plese try again.", (int)StatusCode.UNAUTHORIZED);

            var userDTO = _userService.extractUserDTOFromUser(user);

            var token = _jwtTokenService.GenerateJwtToken(userDTO); ;

            string jwtTokenKey = "jwt-token:{user.Id}";

            TimeSpan expirationTime = ToTimeSpan.covertToTimeSpan(ExpirationType.JWT_TOKEN_VERIFICATION, TimeUnitConversion.DAYS);
            var isStored =  _cacheService.StoreAsync(jwtTokenKey, token, expirationTime);

            if (isStored == null)
                return Result.Failure("Couldn't to verify.Please log in again", (int)StatusCode.INTERNAL_SERVER_ERROR);

            return Result.Success(token, (int)StatusCode.OK);

        }

        public async Task<Result> RefreshJwtTokenAsync(string key)
        {
            var token = await _cacheService.GetAsync(key);

            if (string.IsNullOrEmpty(token))
                return Result.Failure("Invalid or Exprired token", (int)StatusCode.UNAUTHORIZED);

            TimeSpan expireTime = ToTimeSpan.covertToTimeSpan(ExpirationType.JWT_TOKEN_VERIFICATION, TimeUnitConversion.DAYS);

            bool isStored = await _cacheService.StoreAsync(key, token, expireTime);

            if (!isStored)
                return Result.Failure("Couldn't to complete process", (int)StatusCode.INTERNAL_SERVER_ERROR);

            return Result.Success((int)StatusCode.OK_WITH_NO_CONTENT);
        }

        public async Task<Result> ChangePasswordAsync(string newPassword)
        {
           var userId =  _userService.GetLoggedInUserId();
            if (userId == null) 
                return Result.Failure("Unorthorized Request, Please login again.", (int)StatusCode.UNAUTHORIZED);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);
            if (user == null)
                return Result.Failure("User not found", (int)StatusCode.BAD_REQUEST);

            if (!IsValidPassword(newPassword))
                return Result.Failure("Password does not meet verification requerements.", (int)StatusCode.BAD_REQUEST);

            var hashedPassword = _passwordHasher.HashPassword(newPassword);

            var success =  await _authRepository.updatePasswordAsync(user, hashedPassword);

            if (!success)
                return Result.Failure("Failed to update Password", (int)StatusCode.INTERNAL_SERVER_ERROR);

            return Result.Success((int)StatusCode.OK_WITH_NO_CONTENT);
        }

        public async Task<Result> RequestToResetPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return Result.Failure("No user registerd with this email.", (int)StatusCode.UNAUTHORIZED);

            var verificationCode = GenerateVerificationCode();

            TimeSpan expirationTime = ToTimeSpan.covertToTimeSpan(ExpirationType.PASSWORD_VERIFICATION, TimeUnitConversion.MINUTES);

            string verificationKey = "password-reset:{user.Id}";

            bool isSaved = SaveAuthKey(verificationKey, verificationCode, expirationTime);

            if(!isSaved)
                return Result.Failure("Couldn't to complete the request. Try again", (int)StatusCode.INTERNAL_SERVER_ERROR);

            var verificationLink = GenerateVerificationLink(user.Id);


            return await SendPasswordResetLinkToUserEmailAsync(user.Email, verificationLink);
        }

        private async Task<Result> SendPasswordResetLinkToUserEmailAsync(string userEmail, string verificationLink)
        {
            var emailBody = $"Please click the following link to verify your email\n {verificationLink}";
            var emailResult = await _emailService.SendEmailAsync(userEmail, "Account Password Reset Link", emailBody);
            if (emailResult == null)
                return Result.Failure("Coundn't to send the email. Try again", (int)StatusCode.INTERNAL_SERVER_ERROR);

            return Result.Success("Password reset email send to you.", (int)StatusCode.OK);

        }
 
        private static string GenerateVerificationCode()
        {
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();

            return verificationCode;
        }

        private string GenerateVerificationLink(Guid userId)
        {
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            var verificationKey = $"password_reset:{userId}";

            return $"{baseUrl}/password-reset?key={verificationKey}";
        }

        private bool SaveAuthKey(string authKey, string authValue, TimeSpan expiration)
        {
            var isStored =  _cacheService.StoreAsync(authKey, authValue, expiration);

            if(isStored == null) 
                return false;
            return true;
        }

        public async Task<Guid?> IsUserAuthenticated(string key)
        {
            var jwtToken = await _cacheService.GetAsync(key);

            if (string.IsNullOrEmpty(jwtToken))
                return null;

            Guid userId = GetUserIdFromJwtToken(key);
            return userId;
        }

        private Guid GetUserIdFromJwtToken(string JwtToken)
        {
            string userId = JwtToken.Replace("jwt-token:", "");

            return Guid.Parse(userId);
        }

        public async Task<Result> ForgotEmail(string recoveryEmail)
        {
             var email = await _userRepository.GetUserEmailUsingRecoveryEmail(recoveryEmail);
            if (email == null)
                return Result.Failure("No any emails matched to entered email. Try to add correct recovery email", (int)StatusCode.UNAUTHORIZED);

            return Result.Success(email, (int)StatusCode.OK);
        }

        public async Task<Result> ResetPassword(string key, string newPassword)
        {
            var storedCode = await _cacheService.GetAsync(key);

            if (string.IsNullOrEmpty(storedCode))
                return Result.Failure("Invalid or Expired password reset link", (int)StatusCode.BAD_REQUEST);

            string userId = key.Replace("password_reset:", "");
            
            var user =await _userRepository.GetUserByIdAsync(Guid.Parse(userId));

            if (user == null)
                return Result.Failure("User not found", (int)StatusCode.BAD_REQUEST);

            if (!IsValidPassword(newPassword))
                return Result.Failure("Password does not meet verification requerements.", (int)StatusCode.BAD_REQUEST);


            string hashedPassword =  _passwordHasher.HashPassword(newPassword);

            bool isPasswordUpdate =await _authRepository.updatePasswordAsync(user, hashedPassword);

            if (!isPasswordUpdate)
                return Result.Failure("Couldn't to reset password. Try again", (int)StatusCode.INTERNAL_SERVER_ERROR);

            await _cacheService.RemoveAsync(key);

            return Result.Success("Password reset successfully", (int)StatusCode.OK);

        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            if(!password.Any(char.IsLower))
                return false;
            
            if(!password.Any(char.IsUpper))
                return false;
            
            if(!password.Any(char.IsDigit))
                return false;

            if(!password.Any(ch => !char.IsLetterOrDigit(ch))) 
                return false;

            return true;
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
