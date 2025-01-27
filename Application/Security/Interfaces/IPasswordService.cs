using WealthFlow.Shared.Helpers;
using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Security.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);

        bool IsValidPassword(string password);

        string GeneratePasswordResetToken();

        Task<Result> UpdatePasswordIfValidatedAsync(User user, string password);
    }
}
