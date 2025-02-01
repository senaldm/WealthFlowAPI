using System.Security.Claims;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Shared.Helpers;
using System.Net;
using WealthFlow.Domain.Entities.User;
using WealthFlow.Infrastructure.Users.Repositories;

namespace WealthFlow.Application.Users.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AuthServices _authServices;
        private readonly ITokenService _tokenService;

        public UserServices(IUserRepository userRepository, IHttpContextAccessor contextAccessor, 
            AuthServices authServices, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _authServices = authServices;
            _tokenService = tokenService;
        }

        public async Task<Result<Object>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (users == null)
                return Result<Object>.Success("There is no users registerd.", HttpStatusCode.NoContent);
            return Result<Object>.Success(users.Select(user => new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                RecoveryEmail = user.RecoveryEmail,
            }));
        }

        public Guid? GetLoggedInUserId()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId != null ? Guid.Parse(userId) : null;
        }
        public async Task<Result<Object>> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return Result<Object>.Failure("User not fount.", HttpStatusCode.NotFound);

            return Result<Object>.Success(new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                RecoveryEmail = user.RecoveryEmail,
            });
        }

        public async Task<Result<Object>> GetUserDetailsAsync()
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User not found.", HttpStatusCode.NotFound);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);

            return Result<Object>.Success(ExtractPublicUserDetailsFromUser(user));
        }

        private UpdateUserDTO ExtractPublicUserDetailsFromUser(User user)
        {
            var userDetails = new UpdateUserDTO
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RecoveryEmail = user.RecoveryEmail,
            };

            return userDetails;
        }
        public async Task<Result<Object>> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User not found.", HttpStatusCode.Unauthorized);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);

            user.Name = updateUserDTO.Name;
            user.Email = updateUserDTO.Email;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.RecoveryEmail = updateUserDTO.RecoveryEmail;

            bool isUpdate =  await _userRepository.UpdateUserAsync(user);

            if (!isUpdate)
                return Result<Object>.Failure("Coundn't to complete task.Try again.", HttpStatusCode.InternalServerError);

            return Result<Object>.Success(updateUserDTO, HttpStatusCode.OK);
        }

        public async Task<Result<string>> DeleteUserAsync()
        {
            Guid? userId = GetLoggedInUserId();
  
            if (userId != null)
                return Result<string>.Failure("User not found.", HttpStatusCode.Unauthorized);

            bool isDeleted = await _userRepository.DeleteUserAsync(userId.Value);
            
            if(!isDeleted)
                return Result<string>.Failure("Coundn't to complete task.Try again.", HttpStatusCode.InternalServerError);

            return Result<string>.Success(HttpStatusCode.NoContent);
        }

        public async Task<Result<Object>> RequestToUpdateAsync()
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return Result<Object>.Failure("User not found", HttpStatusCode.NotFound);

            Guid id= userId.Value;
            User user = await _userRepository.GetUserByIdAsync(id);

            return Result<Object>.Success(ExtractPublicUserDetailsFromUser(user));

        }

        public UserDTO ExtractUserDTOFromUser(User user)
        {
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
            };

            return userDTO;
        }

    }
}
