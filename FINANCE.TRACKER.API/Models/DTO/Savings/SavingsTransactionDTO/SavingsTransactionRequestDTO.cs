namespace FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO
{
    public class SavingsTransactionRequestDTO
    {
        public int TransactionTypeId { get; set; }
        public int? InvestmentTypeId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; } //1-Active | 0-Closed
        public DateTime DateUsed { get; set; }

    }
}
