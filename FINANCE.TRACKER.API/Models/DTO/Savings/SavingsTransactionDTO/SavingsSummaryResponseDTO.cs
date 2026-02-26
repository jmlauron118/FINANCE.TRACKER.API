namespace FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO
{
    public class SavingsSummaryResponseDTO
    {
        public decimal TotalSavings { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalGains { get; set; }
        public decimal RemainingSavings { get; set; }
    }
}
