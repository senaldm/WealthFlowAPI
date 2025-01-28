using System.Security.Claims;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Shared.Helpers;
using System.Net;

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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                RecoveryEmail = user.RecoveryEmail,
            });
        }

        public Guid? GetLoggedInUserId()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId != null ? Guid.Parse(userId) : null;
        }
        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            return new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                RecoveryEmail = user.RecoveryEmail,
            };
        }

        public async Task<UpdateUserDTO> GetUserDetailsAsync()
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return null;

            var user = await _userRepository.GetUserByIdAsync(userId.Value);

            return ExtractPublicUserDetailsFromUser(user);
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
        public async Task<Result> UpdateUserAsync(UpdateUserDTO updateUserDTO)
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return Result.Failure("User not found.", HttpStatusCode.Unauthorized);

            var user = await _userRepository.GetUserByIdAsync(userId.Value);

            user.Name = updateUserDTO.Name;
            user.Email = updateUserDTO.Email;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.RecoveryEmail = updateUserDTO.RecoveryEmail;

            bool isUpdate =  await _userRepository.UpdateUserAsync(user);

            if (!isUpdate)
                return Result.Failure("Coundn't to complete task.Try again.", HttpStatusCode.InternalServerError);

            return Result.Success(HttpStatusCode.OK);
        }

        public async Task<Result> DeleteUserAsync()
        {
            Guid? userId = GetLoggedInUserId();
  
            if (userId != null)
                return Result.Failure("User not found.", HttpStatusCode.Unauthorized);

            bool isDeleted = await _userRepository.DeleteUserAsync(userId.Value);
            
            if(!isDeleted)
                return Result.Failure("Coundn't to complete task.Try again.", HttpStatusCode.InternalServerError);

            return Result.Success(HttpStatusCode.OK);
        }

        public async Task<UpdateUserDTO> RequestToUpdateAsync()
        {
            Guid? userId = GetLoggedInUserId();

            if (userId == null)
                return null;

            Guid id= userId.Value;
            User user = await _userRepository.GetUserByIdAsync(id);

            return ExtractPublicUserDetailsFromUser(user);

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
