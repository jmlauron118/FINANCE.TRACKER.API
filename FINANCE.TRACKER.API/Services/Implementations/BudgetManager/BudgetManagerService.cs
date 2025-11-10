using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.BudgetManager
{
    public class BudgetManagerService : IBudgetManagerService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public BudgetManagerService(AppDbContext context, IHttpContextAccessor contextAccessor) 
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<BudgetManagerResponseDTO>> GetAllBudgetEntries()
        {
            var budgetEntries =  from bm in _context.BudgetManager
                                   join bc in _context.BudgetCategories on bm.BudgetCagetoryId equals bc.BudgetCategoryId
                                   join ec in _context.ExpensesCategories on bm.ExpenseCategoryId equals ec.ExpensesCategoryId
                                   orderby bm.DateUsed
                                   select new BudgetManagerResponseDTO
                                   {
                                       BudgetManagerId = bm.BudgetManagerId,
                                       BudgetCategoryId = bc.BudgetCategoryId,
                                       BudgetCategoryName = bc.BudgetCategoryName,
                                       ExpenseCategoryId = ec.ExpensesCategoryId,
                                       ExpenseCategoryName = ec.ExpensesCategoryName,
                                       Description = bm.Description,
                                       Amount = bm.Amount,
                                       DateUsed = bm.DateUsed              
                                   };

            return await budgetEntries.ToListAsync();
        }

        public async Task<BudgetManagerResponseDTO> GetBudgetEntryById(int id)
        {
            var budgetEntry = from bm in _context.BudgetManager
                                join bc in _context.BudgetCategories on bm.BudgetCagetoryId equals bc.BudgetCategoryId
                                join ec in _context.ExpensesCategories on bm.ExpenseCategoryId equals ec.ExpensesCategoryId
                                where bm.BudgetManagerId == id
                                orderby bm.DateUsed
                                select new BudgetManagerResponseDTO
                                {
                                    BudgetManagerId = bm.BudgetManagerId,
                                    BudgetCategoryId = bc.BudgetCategoryId,
                                    BudgetCategoryName = bc.BudgetCategoryName,
                                    ExpenseCategoryId = ec.ExpensesCategoryId,
                                    ExpenseCategoryName = ec.ExpensesCategoryName,
                                    Description = bm.Description,
                                    Amount = bm.Amount,
                                    DateUsed = bm.DateUsed
                                };

            var result = await budgetEntry.FirstOrDefaultAsync();

            if (result == null) throw new InvalidOperationException("No budget entry found!");

            return result;
        }

        public async Task<BudgetManagerResponseDTO> AddBudgetEntry(BudgetManagerRequestDTO budgetRequest)
        {
            var newBudgetEntry = new BudgetManagerModel
            {
                BudgetCagetoryId = budgetRequest.BudgetCategoryId,
                ExpenseCategoryId = budgetRequest.ExpenseCategoryId,
                Description = budgetRequest.Description,
                Amount = budgetRequest.Amount,
                DateUsed = budgetRequest.DateUsed,
                CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
                DateCreated = DateTime.Now
            };

            await _context.AddAsync(newBudgetEntry);
            await _context.SaveChangesAsync();

            return await GetBudgetEntryById(newBudgetEntry.BudgetManagerId);
        }

        public async Task AddBudgetEntryBulk(List<BudgetManagerRequestDTO> budgetRequestList)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entries = budgetRequestList.Select(dto => new BudgetManagerModel
                {
                    BudgetCagetoryId = dto.BudgetCategoryId,
                    ExpenseCategoryId = dto.ExpenseCategoryId,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    DateUsed = dto.DateUsed,
                    CreatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0,
                    DateCreated = DateTime.Now
                }).ToList();

                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                await _context.AddRangeAsync(entries);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.AutoDetectChangesEnabled = true;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk saving of budget entries failed! Error: ", ex);
            }
        }

        public async Task<BudgetManagerResponseDTO> ModifyBudgetEntry(BudgetManagerModifyDTO budgetRequest)
        {
            var budgetEntryToUpdate = await _context.BudgetManager.FindAsync(budgetRequest.BudgetManagerId);

            if (budgetEntryToUpdate == null) throw new InvalidOperationException("No budget entry found!");

            budgetEntryToUpdate.BudgetCagetoryId = budgetRequest.BudgetCategoryId;
            budgetEntryToUpdate.ExpenseCategoryId = budgetRequest.ExpenseCategoryId;
            budgetEntryToUpdate.Description = budgetRequest.Description;
            budgetEntryToUpdate.Amount = budgetRequest.Amount;
            budgetEntryToUpdate.DateUsed = budgetRequest.DateUsed;
            budgetEntryToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
            budgetEntryToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetBudgetEntryById(budgetEntryToUpdate.BudgetManagerId);
        }

        public async Task RemoveBudgetEntry(List<BudgetManagerDeleteDTO> idList)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var ids = idList.Select(bm => bm.BudgetManagerId).ToList();
                var entriesToDelete = await _context.BudgetManager
                                            .Where(bm => ids.Contains(bm.BudgetManagerId))
                                            .ToListAsync();

                if (entriesToDelete.Any())
                {
                    _context.BudgetManager.RemoveRange(entriesToDelete);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk deletion of budget entries failed! Error: ", ex);
            }
        }
    }
}
