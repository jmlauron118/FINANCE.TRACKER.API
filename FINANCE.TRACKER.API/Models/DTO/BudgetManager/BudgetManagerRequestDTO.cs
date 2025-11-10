namespace FINANCE.TRACKER.API.Models.DTO.BudgetManager
{
    public class BudgetManagerRequestDTO
    {
        public int BudgetCategoryId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateUsed { get; set; }
    }
}