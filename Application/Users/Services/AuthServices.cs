using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities.Users;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Shared.Helpers;
using WealthFlow.Infrastructure.ExternalServices.MailServices;
using System.Net;
using WealthFlow.Infrastructure.Users.Repositories;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender<User> _emailSender;

        public AuthServices(IAuthRepository authRepository, IUserRepository userRepository, 
            IPasswordService passwordService, ITokenService jwtTokenService, 
            IUserService userService, IConfiguration configuration,
            IEmailService emailService, IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, IMapper mapper, 
            SignInManager<User> signInManager, IEmailSender<User> emailSender)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = jwtTokenService;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<Result<string>>  RegisterAsync([FromBody] UserRegistrationDTO registerUserDTO)
        {
            //bool isUnique = await _userRepository.IsEmailUniqueAsync(registerUserDTO.Email);

            //if (!isUnique)
            //    return Result<string>.Failure("Email is already in use.", HttpStatusCode.Conflict);

            //string hashedPassword = _passwordService.HashPassword(registerUserDTO.Password);

            //var user = new User
            //{
            //    FirstName = registerUserDTO.FirstName,
            //    LastName = registerUserDTO.LastName,
            //    Email = registerUserDTO.Email,
            //    PasswordHash = hashedPassword,
            //    PhoneNumber = string.IsNullOrEmpty(registerUserDTO.PhoneNumber) ? null : registerUserDTO.PhoneNumber,
            //    RecoveryEmail = string.IsNullOrEmpty(registerUserDTO.RecoveryEmail) ? null : registerUserDTO.RecoveryEmail
            //};

            //bool isCreated =  await _authRepository.CreateUserAsync(user);
            //if (!isCreated)
            //    return Result<string>.Failure("Failed to create user due to internal error!", HttpStatusCode.InternalServerError);

            //return Result<string>.Success("User Registers Successfully! ", HttpStatusCode.Created);

            if (registerUserDTO is null)
                return Result<string>.Failure("You must fill the form", HttpStatusCode.BadRequest);

            var isUserExist = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if(isUserExist != null)
                return Result<string>.Failure("Email is already in use.", HttpStatusCode.Conflict);


            var user = _mapper.Map<User>(registerUserDTO);
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            var roleCreate = await _userManager.AddToRoleAsync(user, "User");

            if(!result.Succeeded || !roleCreate.Succeeded)
                return Result<string>.Failure("Failed to create user due to internal error!", HttpStatusCode.InternalServerError);

            return Result<string>.Success("User Registers Successfully! ", HttpStatusCode.Created);

        }

        //public async Task<Result<string>> LoginAsync(string email, string password)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(email);

        //    if (user == null)
        //        return Result<string>.Failure("Invalid Email. Please try again", HttpStatusCode.Unauthorized);

        //    if (!_passwordService.VerifyPassword(password, user.PasswordHash))
        //        return Result<string>.Failure("Invalid password.Plese try again.", HttpStatusCode.Unauthorized);

        //    var userDTO = _userService.ExtractUserDTOFromUser(user);

        //    return await _tokenService.GenerateJwtToken(userDTO);
        //}
       
        //public async Task<Result<string>> ChangePasswordAsync(string newPassword)
        //{         
        //    var userId = _userService.GetLoggedInUserId();
        //    if (userId == null)
        //        return Result<string>.Failure("Unauthorized Request, Please login again.", HttpStatusCode.Unauthorized);

        //    var user = await _userRepository.GetUserByIdAsync(userId.Value);
        //    if (user == null)
        //        return Result<string>.Failure("User not found", HttpStatusCode.NotFound);

        //    return await _passwordService.UpdatePasswordIfValidatedAsync(user, newPassword);

        //}


        //new method
        public async Task<Result<string>> LoginAsync([FromBody] UserLoginDTO loginDTO)
        {

            var result = await _signInManager
                .PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, lockoutOnFailure:false);

            if (result.IsNotAllowed)
                return Result<string>.Failure("Check credentials again.", HttpStatusCode.Unauthorized);

            if (!result.Succeeded)
                return Result<string>.Failure("Internal Server Error", HttpStatusCode.InternalServerError);

            return Result<string>.Success(HttpStatusCode.OK);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
                return result;

            var updateSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);

            if (!updateSecurityStamp.Succeeded)
                return updateSecurityStamp;

            return result;

        }

        public async Task<Result<string>> RequestToResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<string>.Failure("No user registerd with this email.", HttpStatusCode.Unauthorized);

            
            var verificationToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var verificationLink = GenerateVerificationLink(verificationToken);

            await _emailSender.SendPasswordResetLinkAsync(user, user.Email, verificationLink);

            return Result<string>.Success(HttpStatusCode.OK);


        }

        public async Task<Result<string>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
                return Result<string>.Failure("No user registerd with this email.", HttpStatusCode.Unauthorized);

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if(!result.Succeeded)
                return Result<string>.Failure("Couldn't to complete the request. Try again", HttpStatusCode.InternalServerError);

            return Result<String>.Success(HttpStatusCode.OK);

        }

        public async Task<Result<string>> ForgotEmailAsync(string recoveryEmail)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u=> u.RecoveryEmail == recoveryEmail);

            if (user == null)
                return Result<string>.Failure("No Account Associated with this recovery Email", HttpStatusCode.Unauthorized);

            await _emailSender.

        }

        public async Task<Result<string>> LogedOutAsync()
        {
            await _signInManager.SignOutAsync();

            return Result<string>.Success(HttpStatusCode.OK);
        }
        //public async Task<Result<string>> RequestToResetPasswordAsync(string email)
        //{
        //    var user = await _userRepository.GetUserByEmailAsync(email);
        //    if (user == null)
        //        return Result<string>.Failure("No user registerd with this email.", HttpStatusCode.Unauthorized);

        //    string verificationToken = _passwordService.GeneratePasswordResetToken();
        //    bool isSaved = await _tokenService.StorePasswordResetToken(user.Id, verificationToken);

        //    if (!isSaved)
        //        return Result<string>.Failure("Couldn't to complete the request. Try again", HttpStatusCode.InternalServerError);

        //    var verificationLink = GenerateVerificationLink(verificationToken);

        //    return await _emailService.SendPasswordResetLinkAsync(user.Email, verificationLink);
        //}

        private string GenerateVerificationLink(string token)
        {
            var baseUrl = _configuration["AppSettings:FrontendUrl"];
            //var verificationKey = $"password_reset:{key}";

            return $"{baseUrl}/reset-password?token={token}";
        }

        public async Task<Result<string>> ForgotEmail(string recoveryEmail)
        {
            var email = await _userRepository.GetUserEmailUsingRecoveryEmail(recoveryEmail);
            if (email == null)
                return Result<string>.Failure("No any emails matched to entered email. Try to add correct recovery email", HttpStatusCode.Unauthorized);

            return Result<string>.Success(email, HttpStatusCode.OK);
        }

        //public async Task<Result<string>> ResetPassword(string key, string newPassword)
        //{

        //    string userId = await _tokenService.GetPasswordResetTokenIfAny(key);

        //    if (string.IsNullOrEmpty(userId))
        //        return Result<string>.Failure("Invalid or Exprired password reset link", HttpStatusCode.BadRequest);

        //    var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));

        //    if (user == null)
        //        return Result<string>.Failure("User not found", HttpStatusCode.NotFound);

        //    return await _passwordService.UpdatePasswordIfValidatedAsync(user, newPassword);


        //}

        //public async Task<Result<Object>> LogOut()
        //{
        //    Guid? userId = _userService.GetLoggedInUserId();

        //    if (userId == null)
        //        return Result<Object>.Failure("Unauthorized Request, Please login again.", HttpStatusCode.Unauthorized);

        //    await removeTokens();
        //    return Result<Object>.Success(HttpStatusCode.NoContent);
        //}

        private async Task removeTokens()
        {
            Guid? userId = _userService.GetLoggedInUserId() ?? Guid.Empty;
            if (userId == null)
                return;

            await _tokenService.RemoveTokensInCache(userId.Value);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");

        }
    }
}
