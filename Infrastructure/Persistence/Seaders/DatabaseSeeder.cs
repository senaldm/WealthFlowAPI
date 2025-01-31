using WealthFlow.Infrastructure.Persistence.DBContexts;
using WealthFlow.Application.Security.Interfaces;
namespace WealthFlow.Infrastructure.Persistence.Seaders
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IPasswordService _passwordService;

        public DatabaseSeeder(ApplicationDBContext dbContext, IPasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
        }

        public void Seed()
        {
            new ExpenseTypeSeader(_dbContext).Seed();
            new IncomeTypeSeeder(_dbContext).Seed();
            new PaymentMethodSeeder(_dbContext).Seed();
            new AdminDataSeeder(_dbContext, _passwordService).Seed();
        }
    }
}
