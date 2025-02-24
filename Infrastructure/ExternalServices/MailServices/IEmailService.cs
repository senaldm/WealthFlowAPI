﻿using WealthFlow.Shared.Helpers;

namespace WealthFlow.Infrastructure.ExternalServices.MailServices
{
    public interface IEmailService
    {
        Task<Result<String>> SendPasswordResetLinkAsync(string userEmail, string verificationLink);
        Task<bool> SendEmailAsync(string email, string subject, string body);
    }
}
