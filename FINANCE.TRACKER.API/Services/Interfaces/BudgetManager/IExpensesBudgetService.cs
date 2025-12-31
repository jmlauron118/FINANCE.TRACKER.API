using FINANCE.TRACKER.API.Models.DTO.BudgetManager.ExpensesBudget;

namespace FINANCE.TRACKER.API.Services.Interfaces.BudgetManager
{
    public interface IExpensesBudgetService
    {
        Task<IEnumerable<ExpensesBudgetResponseDTO>> GetExpensesBudgetByCategory(int categoryId);
        Task<IEnumerable<ExpensesBudgetCategoryResponseDTO>> GetExpensesBudgetCategory();
        Task AddExpensesBudgetBulk(List<ExpenseBudgetRequestDTO> expensesBudgetRequest, int categoryId);
    }
}
