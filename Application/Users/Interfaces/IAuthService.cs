using System.Threading.Tasks;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Shared.Helpers;
using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Users.Interfaces
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(UserRegistrationDTO registerUserDTO);
        Task<Result> LoginAsync(string email, string password);
        Task<Result> ChangePasswordAsync(string token, string newPassword);
        Task<Result> RequestToResetPasswordAsync(string email);
        Task<Result> ForgotEmail(string recoveryEmail);
        Task<Result> RefreshJwtTokenAsync(string key);
        Task<Result> ResetPassword(string key, string newPassword);
    }
}
