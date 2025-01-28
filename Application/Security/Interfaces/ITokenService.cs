using WealthFlow.Application.Users.DTOs;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.Application.Security.Interfaces
{
    public interface ITokenService
    {
        Task<Result> GenerateJwtToken(UserDTO userDto);
        Task<bool> StorePasswordResetToken(Guid userId, string token);
        Task<string?> GetPasswordResetTokenIfAny(string key);
        Task<Result> RefreshTokenAsync(string refreshKey);
        Task RemoveTokensInCache(Guid userId);
    }
}
