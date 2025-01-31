using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Application.Users.Services;

namespace WealthFlow.Application
{
    public static class ApplicationModel
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserServices>();
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IIncomeService, IIncomeService>();

            return services;
        }
    }
}
