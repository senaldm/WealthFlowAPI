
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
    }
}
