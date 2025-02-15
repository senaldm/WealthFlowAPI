using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Persistence.Seeders
{
    public class ExpenseTypeSeeder
    {
        private readonly ApplicationDBContext _dbContext;

        public ExpenseTypeSeeder(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (!_dbContext.ExpenseTypes.Any())
            {
                _dbContext.ExpenseTypes.AddRange(new List<ExpenseType>
                {
                    new ExpenseType { ExpenceTypeId = 1, ExpenceName = "Utilities" },
                    new ExpenseType { ExpenceTypeId = 2, ExpenceName = "Groceries"},
                    new ExpenseType { ExpenceTypeId = 3, ExpenceName = "Rent"},
                    new ExpenseType { ExpenceTypeId = 4, ExpenceName = "Travel and Transport"},
                    new ExpenseType { ExpenceTypeId = 5, ExpenceName = "BankPayments"}
                });

                _dbContext.SaveChanges();
            }
        }
    }
}
