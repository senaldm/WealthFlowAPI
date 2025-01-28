using WealthFlow.Application.Users.DTOs;
using WealthFlow.Domain.Entities;
using WealthFlow.Shared.Helpers;
namespace WealthFlow.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<Result> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<Result> DeleteUserAsync();
        Guid? GetLoggedInUserId();
        UserDTO ExtractUserDTOFromUser(User user);
        Task<UpdateUserDTO> RequestToUpdateAsync();
        Task<UpdateUserDTO> GetUserDetailsAsync();

    }
}
