using WealthFlow.Application.Users.DTOs;
using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(Guid id);
        Guid? GetLoggedInUserId();
        UserDTO extractUserDTOFromUser(User user);

    }
}
