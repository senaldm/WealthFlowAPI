using WealthFlow.Infrastructure.Persistence.DBContexts;
using WealthFlow.Domain.Entities.Transactions;
namespace WealthFlow.Infrastructure.Persistence.Seeders
{
    public class PaymentMethodSeeder
    {
        private readonly ApplicationDBContext _dbContext;

        public PaymentMethodSeeder(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (!_dbContext.PaymentMethods.Any())
            {
                _dbContext.PaymentMethods.AddRange(new List<PaymentMethod>
                {
                    new PaymentMethod { PaymentMethodId = 1, MethodName = "Cash"},
                    new PaymentMethod { PaymentMethodId = 2, MethodName = "Bank Transfer" },
                    new PaymentMethod { PaymentMethodId = 3, MethodName = "Credit Card" },
                    new PaymentMethod { PaymentMethodId = 4, MethodName = "Debit Card" },
                    new PaymentMethod { PaymentMethodId = 5, MethodName = "Digital Wallet" },
                    new PaymentMethod { PaymentMethodId = 6, MethodName = "Other" }

                });

                _dbContext.SaveChanges();
            }
        }
    }
}
