using System.Threading.Tasks;
using WealthFlow.Application.Users.DTOs;
using WealthFlow.Shared.Helpers;
using WealthFlow.Domain.Entities;

namespace WealthFlow.Application.Users.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> RegisterAsync(UserRegistrationDTO registerUserDTO);
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result<string>> ChangePasswordAsync(string newPassword);
        Task<Result<string>> RequestToResetPasswordAsync(string email);
        Task<Result<string>> ForgotEmail(string recoveryEmail);
        Task<Result<string>> ResetPassword(string key, string newPassword);
        Task<Result<Object>> LogOut();
    }
}
