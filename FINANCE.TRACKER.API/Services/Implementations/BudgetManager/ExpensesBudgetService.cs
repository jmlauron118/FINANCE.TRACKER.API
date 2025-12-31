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

        public async Task AddExpensesBudgetBulk(List<ExpenseBudgetRequestDTO> expensesBudgetRequest, int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException("categoryId must be a positive integer.", nameof(categoryId));

            using var transaction = await _context.Database.BeginTransactionAsync();

            var originalAutoDetect = _context.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                await _context.ExpensesBudget
                    .Where(eb => eb.ExpensesBudgetCategoryId == categoryId && eb.CreatedBy == _userId)
                    .ExecuteDeleteAsync();

                if (expensesBudgetRequest != null && expensesBudgetRequest.Any())
                {
                    var now = DateTime.Now;
                    var newBudgetExpenses = expensesBudgetRequest.Select(eb => new ExpensesBudgetModel
                    {
                        ExpensesBudgetCategoryId = categoryId,
                        Description = eb.Description,
                        Amount = eb.Amount,
                        CreatedBy = _userId,
                        DateCreated = now
                    }).ToList();

                    _context.ChangeTracker.AutoDetectChangesEnabled = false;
                    await _context.ExpensesBudget.AddRangeAsync(newBudgetExpenses);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk saving of expenses budget failed! Error: ", ex);
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = originalAutoDetect;
            }
        }
    }
}
