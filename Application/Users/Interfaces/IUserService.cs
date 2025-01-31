using WealthFlow.Application.Users.DTOs;
using WealthFlow.Domain.Entities.User;
using WealthFlow.Shared.Helpers;
namespace WealthFlow.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<Result<Object>> GetUserByIdAsync(Guid id);
        Task<Result<Object>> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<Result<Object>> GetAllUsersAsync();
        Task<Result<string>> DeleteUserAsync();
        Guid? GetLoggedInUserId();
        UserDTO ExtractUserDTOFromUser(User user);
        Task<Result<Object>> RequestToUpdateAsync();
        Task<Result<Object>> GetUserDetailsAsync();

    }
}
