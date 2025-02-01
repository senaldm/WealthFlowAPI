using WealthFlow.Application.Transactions.Interfaces;
using WealthFlow.Application.Users.Interfaces;
using WealthFlow.Application.Users.Services;
using WealthFlow.Application.Transactions.Services;

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

            return services;
        }
    }
}
