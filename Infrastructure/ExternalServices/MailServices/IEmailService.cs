namespace WealthFlow.Infrastructure.ExternalServices.MailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string body);
    }
}
