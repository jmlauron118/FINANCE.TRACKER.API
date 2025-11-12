namespace FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry
{
    public class BudgetEntryResponseDTO
    {
        public int BudgetEntryId { get; set; }
        public int BudgetCategoryId { get; set; }
        public string? BudgetCategoryName { get; set; }
        public int? ExpenseCategoryId { get; set; }
        public string? ExpenseCategoryName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime DateUsed { get; set; }
    }
}
