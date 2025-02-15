using WealthFlow.Domain.Entities.Users;

namespace WealthFlow.Domain.Entities.Transactions
{
    public class Expense
    {
        public Expense() { 

            ExpenseId = Guid.NewGuid();
        }

        public Guid ExpenseId { get; set; }
        public Guid UserId { get; set; }

        public string ExpenseName { get; set; }
        public string ExpenseDescription { get; set; }
        public decimal ExpenseAmount { get; set; }

        public int PaymentMethodId { get; set; }

        public int ExpenseTypeId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt   { get; set; }

        public User User { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
