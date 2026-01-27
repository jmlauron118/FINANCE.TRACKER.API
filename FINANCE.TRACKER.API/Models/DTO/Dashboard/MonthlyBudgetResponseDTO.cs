namespace FINANCE.TRACKER.API.Models.DTO.Dashboard
{
    public class MonthlyBudgetResponseDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal CurrentBalance { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

    }
}
