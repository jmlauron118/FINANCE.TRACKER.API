namespace FINANCE.TRACKER.API.Models.DTO.BudgetManager.ExpensesBudget
{
    public class ExpenseBudgetRequestDTO
    {
        public int ExpensesBudgetCategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
