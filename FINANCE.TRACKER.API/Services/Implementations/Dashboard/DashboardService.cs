using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using FINANCE.TRACKER.API.Models.DTO.Dashboard;
using FINANCE.TRACKER.API.Services.Interfaces.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public DashboardService(AppDbContext context, IHttpContextAccessor contextAccessor) { 
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        public async Task<SummaryResponseDTO> GetSummary()
        {
            var budgetEntries = await (from be in _context.BudgetEntries
                                   join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                   join ec in _context.ExpensesCategories on be.ExpenseCategoryId equals ec.ExpensesCategoryId into ecGroup
                                   from ec in ecGroup.DefaultIfEmpty()
                                   where be.CreatedBy == _userId
                                   orderby be.DateUsed
                                   select new BudgetEntryResponseDTO
                                   {
                                       BudgetEntryId = be.BudgetEntryId,
                                       BudgetCategoryId = bc.BudgetCategoryId,
                                       BudgetCategoryName = bc.BudgetCategoryName,
                                       ExpenseCategoryId = ec.ExpensesCategoryId,
                                       ExpenseCategoryName = ec.ExpensesCategoryName,
                                       Description = be.Description,
                                       Amount = be.Amount,
                                       DateUsed = be.DateUsed
                                   }).ToListAsync();

            var totalIncome = budgetEntries
                .Where(e => e.BudgetCategoryName.ToLower().Contains("income"))
                .Sum(e => e.Amount ?? 0m);

            var totalSavings = budgetEntries
                .Where(e => e.BudgetCategoryName.ToLower().Contains("savings"))
                .Sum(e => e.Amount ?? 0m);

            var currentBalance = totalIncome - totalSavings - budgetEntries
                .Where(e => e.BudgetCategoryName.ToLower().Contains("expense"))
                .Sum(e => e.Amount ?? 0m);

            var latestMonth = await _context.BudgetEntries
                .Where(be => be.CreatedBy == _userId)
                .MaxAsync(be => be.DateUsed);

            var startOfMonth = new DateTime(latestMonth.Year, latestMonth.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var monthlyBudget = budgetEntries
                .Where(e => e.BudgetCategoryName.ToLower().Contains("income"))
                .Where(e => e.DateUsed >= startOfMonth && e.DateUsed < startOfNextMonth)
                .Sum(e => e.Amount ?? 0m);

            var monthlyExpenses = budgetEntries
                .Where(e => !e.BudgetCategoryName.ToLower().Contains("income"))
                .Where(e => e.DateUsed >= startOfMonth && e.DateUsed < startOfNextMonth)
                .Sum(e => e.Amount ?? 0m);

            var spendingStatus = GetSpendingStatus(monthlyBudget, monthlyExpenses);

            return new SummaryResponseDTO
            {
                TotalIncome = totalIncome,
                TotalSavings = totalSavings,
                CurrentBalance = currentBalance,
                SpendingStatus = spendingStatus
            };
        }

        public async Task<IEnumerable<RecentTransactionsResponseDTO>> GetRecentTransactions()
        {
            var recentTransactions = await (from be in _context.BudgetEntries
                                            join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                            where be.CreatedBy == _userId
                                            orderby be.DateUsed descending, be.DateCreated descending
                                            select new RecentTransactionsResponseDTO
                                            {
                                                BudgetEntryId = be.BudgetEntryId,
                                                BudgetCategoryId = bc.BudgetCategoryId,
                                                BudgetCategoryName = bc.BudgetCategoryName,
                                                Description = be.Description,
                                                Amount = be.Amount,
                                                DateUsed = be.DateUsed,
                                                DateCreated = be.DateCreated
                                            }).Take(10).ToListAsync();

            return recentTransactions;
        }

        public async Task<IEnumerable<YTDIncomeResponseDTO>> GetYTDIncome()
        {
            int currentYear = DateTime.Now.Year;

            var monthlyIncome = await (from be in _context.BudgetEntries
                  join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                  where be.CreatedBy == _userId
                  && bc.BudgetCategoryName.ToLower().Contains("income")
                  && be.DateUsed.Year == currentYear
                  group be by be.DateUsed.Month into g
                  select new
                  {
                      Month = g.Key,
                      TotalIncome = g.Sum(x => x.Amount)
                  })
                  .OrderBy(x => x.Month)
                  .ToListAsync();


            return monthlyIncome.Select(mi => new YTDIncomeResponseDTO
            {
                Month = new DateTime(currentYear, mi.Month, 1).ToString("MMM"),
                TotalIncome = mi.TotalIncome
            });
        }

        public async Task<MonthlyBudgetResponseDTO> GetMonthlyBudget()
        {
            var latestMonth = await _context.BudgetEntries
                .Where(be => be.CreatedBy == _userId)
                .MaxAsync(be => be.DateUsed);

            var startOfMonth = new DateTime(latestMonth.Year, latestMonth.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var monthlyBudgetData = await (from be in _context.BudgetEntries
                                           join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                           where be.CreatedBy == _userId
                                           && (be.DateUsed >= startOfMonth && be.DateUsed < startOfNextMonth)
                                           select new BudgetEntryResponseDTO
                                           {
                                               BudgetEntryId = be.BudgetEntryId,
                                               BudgetCategoryId = bc.BudgetCategoryId,
                                               BudgetCategoryName = bc.BudgetCategoryName,
                                               Amount = be.Amount,
                                               DateUsed = be.DateUsed
                                           }).ToListAsync();

            var totalIncome = monthlyBudgetData.Where(e => e.BudgetCategoryName.ToLower().Contains("income")).Sum(e => e.Amount ?? 0m);
            var totalSavings = monthlyBudgetData.Where(e => e.BudgetCategoryName.ToLower().Contains("savings")).Sum(e => e.Amount ?? 0m);
            var totalExpenses = monthlyBudgetData.Where(e => e.BudgetCategoryName.ToLower().Contains("expenses")).Sum(e => e.Amount ?? 0m);
            var currentBalance = totalIncome - (totalSavings + totalExpenses);

            return new MonthlyBudgetResponseDTO
            {
                TotalIncome = totalIncome,
                TotalSavings = totalSavings,
                TotalExpenses = totalExpenses,
                CurrentBalance = currentBalance,
                startDate = startOfMonth.ToString("MMM dd, yyyy"),
                endDate = new DateTime(latestMonth.Year, latestMonth.Month, DateTime.DaysInMonth(latestMonth.Year, latestMonth.Month)).ToString("MMM dd, yyyy")
            };
        }

        public async Task<IEnumerable<ExpensesByCategoryResponseDTO>> GetExpensesByCategory()
        {
            var latestMonth = await _context.BudgetEntries
                .Where(be => be.CreatedBy == _userId && be.ExpenseCategoryId != null)
                .MaxAsync(be => be.DateUsed);

            var startOfMonth = new DateTime(latestMonth.Year, latestMonth.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);
            var expensesData = await (from be in _context.BudgetEntries
                                      join ec in _context.ExpensesCategories on be.ExpenseCategoryId equals ec.ExpensesCategoryId
                                      where be.CreatedBy == _userId && be.ExpenseCategoryId != null
                                      && (be.DateUsed >= startOfMonth && be.DateUsed < startOfNextMonth)
                                      group be by ec.ExpensesCategoryName into g
                                      select new ExpensesByCategoryResponseDTO
                                      {
                                          ExpenseCategoryName = g.Key,
                                          Amount = g.Sum(be => be.Amount),
                                      }).ToListAsync();

            return expensesData;
        }

        public async Task<IEnumerable<ActivityResponseDTO>> GetActivity()
        {
            int currentYear = DateTime.Now.Year;
            var activityData =  (from be in _context.BudgetEntries
                                      join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                      where be.CreatedBy == _userId && be.DateUsed.Year == currentYear
                                      select new
                                      {
                                          be.DateUsed,
                                          bc.BudgetCategoryId,
                                          bc.BudgetCategoryName,
                                          be.Amount
                                      }).AsEnumerable()
                                      .GroupBy(x => new
                                      {
                                          Month = x.DateUsed.ToString("yyyy-MM"),
                                          CategoryType = x.BudgetCategoryName.ToLower().Contains("income") ? "Income" : "Expenses"
                                      })
                                      .Select(g => new ActivityResponseDTO
                                      {
                                          Month = g.Key.Month,
                                          Category = g.Key.CategoryType,
                                          Amount = g.Sum(x => x.Amount)
                                      }).ToList();

            return activityData;
        }

        private string GetSpendingStatus(decimal budget, decimal expenses)
        {
            var ratio = budget == 0 ? 0 : (expenses / budget) * 100;

            string status = budget switch
            {
                _ when budget == 0 => "No Data",
                _ when ratio <= 50 => "Healthy",
                _ when ratio <= 75 => "Good",
                _ when ratio <= 100 => "On Track",
                _ when ratio <= 120 => "Overspend",
                _ => "Critical"
            };

            return status;
        }
    }
}
