using WealthFlow.Infrastructure.Persistence.DBContexts;
using WealthFlow.Infrastructure.Transactions.Repositories;
using WealthFlow.Infrastructure.Users.Repositories;

namespace WealthFlow.Infrastructure
{
    public static class InfrastructureModel
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            return services;
        }
    }
}
