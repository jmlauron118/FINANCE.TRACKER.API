namespace FINANCE.TRACKER.API.Models.DTO.Dashboard
{
    public class RecentTransactionsResponseDTO
    {
        public int BudgetEntryId { get; set; }
        public int BudgetCategoryId { get; set; }
        public string? BudgetCategoryName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime DateUsed { get; set; }

    }
}
