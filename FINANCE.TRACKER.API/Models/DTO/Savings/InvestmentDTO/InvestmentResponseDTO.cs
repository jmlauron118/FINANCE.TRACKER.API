namespace FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO
{
    public class InvestmentResponseDTO : InvestmentRequestDTO
    {
        public int InvestmentId { get; set; }
        public string InvestmentTypeName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public int ReturnTransactionId { get; set; }
        public DateTime DateUsed { get; set; }
    }
}
