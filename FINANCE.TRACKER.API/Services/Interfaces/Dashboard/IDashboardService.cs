using FINANCE.TRACKER.API.Models.DTO.Dashboard;

namespace FINANCE.TRACKER.API.Services.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<SummaryResponseDTO> GetSummary();
        Task<IEnumerable<RecentTransactionsResponseDTO>> GetRecentTransactions();
        Task<IEnumerable<YTDIncomeResponseDTO>> GetYTDIncome();
        Task<MonthlyBudgetResponseDTO> GetMonthlyBudget();
        Task<IEnumerable<ExpensesByCategoryResponseDTO>> GetExpensesByCategory();
        Task<IEnumerable<ActivityResponseDTO>> GetActivity();
    }
}
