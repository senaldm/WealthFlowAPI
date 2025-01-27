using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<string?> GetUserEmailUsingRecoveryEmail(string recoveryEmail);
    }
}
