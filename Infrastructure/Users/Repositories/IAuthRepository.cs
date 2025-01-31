using WealthFlow.Application.Users.DTOs;
using WealthFlow.Domain.Entities.User;

namespace WealthFlow.Infrastructure.Users.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> CreateUserAsync(User user);
        Task<bool> SaveVerificationTokenAsync(string token);
        Task<bool> DeleteVerificationTokenAsync(Guid userId);
        Task<bool> ValidateToken(string token, Guid userId);
        Task<bool> verifyToken(string refreshToken);
        Task<bool> verifyCodeAsync(int code);
        Task<bool> ValidateEmailAsync(string email);
        Task<bool> updatePasswordAsync(User user, string hashedPassword);

    }
}
