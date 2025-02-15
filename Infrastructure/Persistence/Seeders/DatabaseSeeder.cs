using WealthFlow.Infrastructure.Persistence.DBContexts;
using WealthFlow.Application.Security.Interfaces;
using WealthFlow.Infrastructure.Persistence.Seeders;

namespace WealthFlow.Infrastructure.Persistence.Seeders
{
    public class DatabaseSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Seed()
        {
            // Create a new scope to isolate seeding operations
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

                // Call individual seeder methods
                new ExpenseTypeSeeder(dbContext).Seed();
                new IncomeTypeSeeder(dbContext).Seed();
                new PaymentMethodSeeder(dbContext).Seed();
                new AdminDataSeeder(dbContext, passwordService).Seed();
            }
        }
    }
}
