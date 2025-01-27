using System.Security.Claims;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Domain.Entities;
using WealthFlow.Application.Security.Interfaces;

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

        public async Task<bool> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return false;

            user.Name = updateUserDTO.Name;
            user.Email = updateUserDTO.Email;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.RecoveryEmail = updateUserDTO.RecoveryEmail;

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user != null)
                return false;

            bool isDeleted = await _userRepository.DeleteUserAsync(userId);

            return isDeleted;
        }

        public async Task<UserDTO> RequestToUpdate(string token)
        {
            Guid? userId = await _tokenService.GetUserIdFromJwtTokenIfValidated(token);

            if (userId == null)
                return null;
            Guid id= userId.Value;
            User user = await _userRepository.GetUserByIdAsync(id); 

            return  extractUserDTOFromUser(user);

        }

        public UserDTO extractUserDTOFromUser(User user)
        {
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            return userDTO;
        }

    }
}
