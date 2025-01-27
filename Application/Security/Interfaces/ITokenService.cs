using WealthFlow.Application.Users.DTOs;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.Application.Security.Interfaces
{
    public interface ITokenService
    {
        Task<Result> GenerateJwtToken(UserDTO userDto);

        string GetJwtToken();

        Task<bool> IsValidatedJwtToken(string jwtKey);

        Task<Guid?> GetUserIdFromJwtTokenIfValidated(string jwtKey);

        Task<bool> StorePasswordResetToken(Guid userId, string token);

        Task<string?> GetPasswordResetTokenIfAny(string key);

        Task<Result> RefreshJwtTokenAsync(string key);
    }
}
