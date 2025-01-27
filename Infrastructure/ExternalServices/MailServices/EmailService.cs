
using System.Net;
using WealthFlow.Shared.Helpers;

namespace WealthFlow.Infrastructure.ExternalServices.MailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> SendEmailAsync(string email, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> SendPasswordResetLinkAsync(string userEmail, string verificationLink)
        {
            string emailBody = $"Click <a href='{verificationLink}'>here</a> to reset your password.\n This link expires in 15 minutes.";
            bool emailResult = await SendEmailAsync(userEmail, "Account Password Reset Link", emailBody);
            if (!emailResult)
                return Result.Failure("Coundn't to send the email. Try again", HttpStatusCode.InternalServerError);

            return Result.Success("Password reset email send to you.", HttpStatusCode.OK);
        }
    }
}
