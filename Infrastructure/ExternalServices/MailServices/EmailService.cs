
using Microsoft.AspNetCore.Identity;
using System.Net;
using WealthFlow.Shared.Helpers;
using WealthFlow.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WealthFlow.Infrastructure.ExternalServices.MailServices
{
    public class EmailService : IEmailSender
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(string apiKey, string senderEmail, string senderName)
        {
            _apiKey = apiKey;
            _senderEmail = senderEmail;
            _senderName = senderName;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);
            var response = await client.SendEmailAsync(msg);
        }


        //public Task<bool> SendEmailAsync(string email, string subject, string body)
        //{
        //    throw new NotImplementedException();
        //}


        //public async Task<Result<String>> SendPasswordResetLinkAsync(string userEmail, string verificationLink)
        //{
        //    string emailBody = $"Click <a href='{verificationLink}'>here</a> to reset your password.\n This link expires in 15 minutes.";
        //    bool emailResult = await SendEmailAsync(userEmail, "Account Password Reset Link", emailBody);
        //    if (!emailResult)
        //        return Result<String>.Failure("Coundn't to send the email. Try again", HttpStatusCode.InternalServerError);

        //    return Result<String>.Success("Password reset email send to you.", HttpStatusCode.OK);
        //}
    }
}
