using WealthFlow.Domain.Entities.Transactions;
using WealthFlow.Infrastructure.Persistence.DBContexts;

namespace WealthFlow.Infrastructure.Persistence.Seaders
{
    public class IncomeTypeSeeder
    {
        private readonly ApplicationDBContext _dbContext;

        public IncomeTypeSeeder(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public void Seed()
        {
            if (!_dbContext.IncomeTypes.Any())
            {
                _dbContext.IncomeTypes.AddRange(new List<IncomeType>
                {
                    new IncomeType { IncomeTypeId = 1, TypeName = "Salary" },
                    new IncomeType { IncomeTypeId = 2, TypeName = "Bonus" },
                    new IncomeType { IncomeTypeId = 3, TypeName = "Business" },
                    new IncomeType { IncomeTypeId = 4, TypeName = "Investment" },
                    new IncomeType { IncomeTypeId = 5, TypeName = "Other" }
                });

                _dbContext.SaveChanges();
            }
        }
    }
}
