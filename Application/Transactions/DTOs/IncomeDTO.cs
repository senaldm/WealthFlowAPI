namespace WealthFlow.Application.Transactions.DTOs
{
    public class IncomeDTO
    {
        public Guid IncomeId { get; set; }
        public int IncomeTypeId { get; set; }
        public string IncomeName { get; set; }
        public string IncomeDescription { get; set; } = string.Empty;
        public decimal IncomeAmount { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime ReceiveDate { get; set; }

    }
}
