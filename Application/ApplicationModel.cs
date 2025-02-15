using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Application.Users.Services;
using WealthFlow.Application.Transactions.Services;
using WealthFlow.Application.Caching.Interfaces;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Application.Security.Services;
using WealthFlow.Infrastructure.Caching;
using WealthFlow.Infrastructure.ExternalServices.MailServices;

namespace WealthFlow.Application
{
    public static class ApplicationModel
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserServices>();
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
