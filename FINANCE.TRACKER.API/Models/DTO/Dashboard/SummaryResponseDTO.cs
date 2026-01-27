namespace FINANCE.TRACKER.API.Models.DTO.Dashboard
{
    public class SummaryResponseDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal CurrentBalance { get; set; }
        public string SpendingStatus { get; set; } = string.Empty;
    }
}
