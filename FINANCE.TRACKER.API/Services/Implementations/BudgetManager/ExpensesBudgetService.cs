using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.ExpensesBudget;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.EntityFrameworkCore;

namespace FINANCE.TRACKER.API.Services.Implementations.BudgetManager
{
    public class ExpensesBudgetService : IExpensesBudgetService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public ExpensesBudgetService(
                AppDbContext context,
                IHttpContextAccessor contextAccessor
            )
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        #region Expenses Budget
        public async Task<IEnumerable<ExpensesBudgetResponseDTO>> GetExpensesBudgetByCategory(int categoryId)
        {
            return await _context.ExpensesBudget
                .Where(eb => eb.ExpensesBudgetCategoryId == categoryId && eb.CreatedBy == _userId)
                .Select(eb => new ExpensesBudgetResponseDTO
                {
                    Id = eb.Id,
                    ExpensesBudgetCategoryId = eb.ExpensesBudgetCategoryId,
                    Description = eb.Description,
                    Amount = eb.Amount
                }).ToListAsync();
        }

        public async Task<IEnumerable<ExpensesBudgetCategoryResponseDTO>> GetExpensesBudgetCategory()
        {
            return await _context.ExpensesBudgetCategories
                .Select(eb => new ExpensesBudgetCategoryResponseDTO
                {
                    ExpensesBudgetCategoryId = eb.ExpensesBudgetCategoryId,
                    Name = eb.ExpensesBudgetCategoryName,
                    Description = eb.Description
                }).ToListAsync();
        }

        public async Task AddExpensesBudgetBulk(List<ExpenseBudgetRequestDTO> ExpensesBudgetRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newBudgetExpenses = ExpensesBudgetRequest.Select(eb => new ExpensesBudgetModel
                {
                    ExpensesBudgetCategoryId = eb.ExpensesBudgetCategoryId,
                    Description = eb.Description,
                    Amount = eb.Amount,
                    CreatedBy = _userId,
                    DateCreated = DateTime.Now
                }).ToList();

                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                await _context.ExpensesBudget.AddRangeAsync(newBudgetExpenses);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.AutoDetectChangesEnabled = true;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk saving of expenses budget failed! Error: ", ex);
            }
        }

        public async Task ModifyExpensesBudgetBulk(List<ExpensesBudgetModifyDTO> ExpensesBudgetRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            { 
                var budgetExpensesToUpdate = _context.ExpensesBudget
                    .Where(eb => ExpensesBudgetRequest.Select(b => b.Id).Contains(eb.Id) && eb.CreatedBy == _userId)
                    .ToList();

                foreach (var expense in budgetExpensesToUpdate)
                {
                    var updatedData = ExpensesBudgetRequest.First(b => b.Id == expense.Id);

                    expense.ExpensesBudgetCategoryId = updatedData.ExpensesBudgetCategoryId;
                    expense.Description = updatedData.Description;
                    expense.Amount = updatedData.Amount;
                    expense.UpdatedBy = _userId;
                    expense.DateUpdated = DateTime.Now;
                }

                _context.ExpensesBudget.UpdateRange(budgetExpensesToUpdate);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk modification of expenses budget failed! Error: ", ex);
            }
        }

        public async Task RemoveExpensesBudgetBulk(List<ExpensesBudgetDeleteDTO> idList)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var budgetExpensesToDelete = await _context.ExpensesBudget
                    .Where(eb => idList.Select(b => b.Id).Contains(eb.Id) && eb.CreatedBy == _userId)
                    .ToListAsync();

                if (!budgetExpensesToDelete.Any()) throw new InvalidOperationException("No expenses budget found to delete!");

                _context.ExpensesBudget.RemoveRange(budgetExpensesToDelete);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk deletion of expenses budget failed! Error: ", ex);
            }
        }
        #endregion Expenses Budget
    }
}
