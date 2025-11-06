using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.Category;
using FINANCE.TRACKER.API.Models.DTO.Category.ExpenseCategoryDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Category
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ExpenseCategoryService(AppDbContext context, IHttpContextAccessor contextAccessor) {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<ExpenseCategoryResponseDTO>> GetAllExpenseCategories(int status)
        {
            return await _context.ExpensesCategories
                .Where(ec => status == 2 || ec.IsActive == status)
                .Select(ec => new ExpenseCategoryResponseDTO
                {
                    ExpenseCategoryId = ec.ExpensesCategoryId,
                    ExpenseCategoryName = ec.ExpensesCategoryName,
                    ExpenseCategoryDescription = ec.ExpensesCategoryDescription,
                    IsActive = ec.IsActive
                }).ToListAsync();
        }

        public async Task<ExpenseCategoryResponseDTO> GetExpenseCategoryById(int id)
        {
            var expenseCategory = await _context.ExpensesCategories
                .Where(ec => ec.ExpensesCategoryId == id)
                .Select(ec => new ExpenseCategoryResponseDTO
                {
                    ExpenseCategoryId = ec.ExpensesCategoryId,
                    ExpenseCategoryName = ec.ExpensesCategoryName,
                    ExpenseCategoryDescription = ec.ExpensesCategoryDescription,
                    IsActive = ec.IsActive
                }).FirstOrDefaultAsync();

            if (expenseCategory == null) throw new InvalidOperationException("Expense category not found");

            return expenseCategory;
        }

        public async Task<ExpenseCategoryResponseDTO> AddExpenseCategory(ExpenseCategoryRequestDTO expenseCategory)
        {
            var existingExpenseCategory = await _context.ExpensesCategories.FirstOrDefaultAsync(ec => ec.ExpensesCategoryName == expenseCategory.ExpenseCategoryName);

            if(existingExpenseCategory != null) throw new InvalidOperationException("Expense category already exist");

            var newExpenseCategory = new ExpensesCategoryModel
            {
                ExpensesCategoryName = expenseCategory.ExpenseCategoryName,
                ExpensesCategoryDescription = expenseCategory.ExpenseCategoryDescription,
                IsActive = expenseCategory.IsActive,
                CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
                DateCreated = DateTime.Now
            };

            await _context.ExpensesCategories.AddAsync(newExpenseCategory);
            await _context.SaveChangesAsync();

            return await this.GetExpenseCategoryById(newExpenseCategory.ExpensesCategoryId);
        }

        public async Task<ExpenseCategoryResponseDTO> ModifyExpenseCategory(ExpenseCategoryModifyDTO expenseCategory)
        {
            var existingExpenseCategory = await _context.ExpensesCategories.FirstOrDefaultAsync(ec => 
                ec.ExpensesCategoryName == expenseCategory.ExpenseCategoryName && 
                ec.ExpensesCategoryId != expenseCategory.ExpenseCategoryId
            );

            if (existingExpenseCategory != null)
            {
                throw new InvalidOperationException("Expense category already exist");
            }

            var expenseCategoryToUpdate = await _context.ExpensesCategories.FindAsync(expenseCategory.ExpenseCategoryId);

            if (expenseCategoryToUpdate == null) throw new InvalidOperationException("Expense category not found");

            expenseCategoryToUpdate.ExpensesCategoryName = expenseCategory.ExpenseCategoryName;
            expenseCategoryToUpdate.ExpensesCategoryDescription = expenseCategory.ExpenseCategoryDescription;
            expenseCategoryToUpdate.IsActive = expenseCategory.IsActive;
            expenseCategoryToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            expenseCategoryToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await this.GetExpenseCategoryById(expenseCategoryToUpdate.ExpensesCategoryId);
        }
    }
}
