using WealthFlow.Domain.Entities.Users;

namespace WealthFlow.Domain.Entities.Transactions
{
    public class Income
    {
        public Income() 
        {
            IncomeId = Guid.NewGuid();
        }

        public Guid IncomeId { get; set; }
        public Guid UserId { get; set; }

        public string IncomeName { get; set; }
        public int IncomeTypeId { get; set; }
        public string IncomeDescription { get; set; } = string.Empty;
        public decimal IncomeAmount { get; set; }
        public int PaymentMethodId { get; set; }

        public DateTime ReceiveDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;

        public User User { get; set; }
        public IncomeType IncomeType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
