namespace FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO
{
    public class InvestmentResponseDTO
    {
        public int InvestmentId { get; set; }
        public DateTime InvestmentDate { get; set; }
        public string InvestmentTypeName { get; set; }
        public string Description { get; set; }
        public decimal InvestmentAmount { get; set; }
        public int ReturnTransactionId { get; set; }
        public decimal? RealizedAmount { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
