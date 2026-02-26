namespace FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO
{
    public class ReturnTransactionResponseDTO
    {
        public int TransactionId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateUsed { get; set; }
    }
}
