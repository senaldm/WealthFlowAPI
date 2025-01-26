using System.Globalization;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Shared.Helpers;
using WealthFlow.Infrastructure.ExternalServices.MailServices;

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

        public AuthServices(IAuthRepository authRepository, IUserRepository userRepository, 
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, 
            IUserService userService, IConfiguration configuration, IEmailService emailService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<Result>  RegisterAsync(UserRegistrationDTO registerUserDTO)
        {
            bool isUnique = await _userRepository.IsEmailUniqueAsync(registerUserDTO.Email);

            if (!isUnique)
                return Result.Failure("Email is already in use.", 409);

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
                return Result.Failure("Failed to create user due to internal error!", 500);

            return Result.Success(201);
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                return Result.Failure("Invalid Email. Please try again", 401);

            if (!_passwordHasher.VerifyPassword(password, user.Password))
                return Result.Failure("Invalid password.Plese try again.", 401);

            var userDTO = _userService.extractUserDTOFromUser(user);
            var token = GenerateJwtTokenAsync(userDTO);

            return Result.Success(token, 200);

        }

        public  string GenerateJwtTokenAsync(UserDTO userDTO)
        {
            return  _jwtTokenService.GenerateJwtToken(userDTO);
        }

        public Task<UserDTO> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> ChangePasswordAsync(string newPassword)
        {
           var userId =  _userService.GetLoggedInUserId();
            if (userId == null) 
                return Result.Failure("Unorthorized Request, Please login again.", 401);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);
            if (user == null)
                return Result.Failure("User not found", 400);

            var hashedPassword = _passwordHasher.HashPassword(newPassword);

            var success =  await _authRepository.updatePasswordAsync(user, hashedPassword);

            if (!success)
                return Result.Failure("Failed to update Password", 500);

            return Result.Success(204);
        }

        public async Task<Result> RequestToResetPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return Result.Failure("No user registerd with this email.", 401);

            return await SendPasswordResetLinkToUserByMailAsync(user);
        }

        public async Task<Result> SendPasswordResetLinkToUserByMailAsync(User user)
        {

            var verificationCode = GenerateVerificationCode();
            var verificationLink = GenerateVerificationLink(user.Id, verificationCode);

            var emailBody = $"Please click the following link to verify your email\n {verificationLink}";
            var emailResult = await _emailService.SendEmailAsync(user.Email, "Account Password Reset Link", emailBody);
            if (emailResult == null)
                return Result.Failure("Coundn't to send the email. Try again", 500);

            return Result.Success("Password reset email send to you.", 200);





        }
 
        private string GenerateVerificationCode()
        {
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();

            return verificationCode;
        }

        private string GenerateVerificationLink(Guid userId, string verificationCode)
        {
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            return $"{baseUrl}/verify-email?userId={userId}&code={verificationCode}";
        }

        public Task<bool> RequestToResetEmail(string recoveryEmail)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetVerificationCodeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
