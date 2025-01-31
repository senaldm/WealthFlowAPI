namespace WealthFlow.Domain.Entities.Transactions
{
    public class Income
    {

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

    }
}
