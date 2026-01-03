using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.ExpensesBudget;
using FINANCE.TRACKER.API.Repositories;
using FINANCE.TRACKER.API.Services.Interfaces.BudgetManager;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.BudgetManager
{
    public class BudgetEntryService : IBudgetEntryService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public BudgetEntryService(AppDbContext context, IHttpContextAccessor contextAccessor) 
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = (int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0);
        }

        public async Task<PagedList<BudgetEntryResponseDTO>> GetAllBudgetEntries(BudgetEntryParameters budgetEntryParameters)
        {
            var searchValue = budgetEntryParameters.Search?.Trim().ToLower();
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var budgetEntries = from be in _context.BudgetEntries
                                join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                join ec in _context.ExpensesCategories on be.ExpenseCategoryId equals ec.ExpensesCategoryId into ecGroup
                                from ec in ecGroup.DefaultIfEmpty()
                                where (string.IsNullOrEmpty(searchValue) || (be.Description != null && be.Description.ToLower().Contains(searchValue))
                                    || (bc.BudgetCategoryName != null && bc.BudgetCategoryName.ToLower().Contains(searchValue))
                                    || (ec.ExpensesCategoryName != null && ec.ExpensesCategoryName.ToLower().Contains(searchValue)))
                                    && be.CreatedBy == _userId
                                    && (budgetEntryParameters.Sorted || be.DateUsed >= startOfMonth && be.DateUsed < startOfNextMonth)
                                orderby be.DateUsed descending
                                select new BudgetEntryResponseDTO
                                {
                                    BudgetEntryId = be.BudgetEntryId,
                                    BudgetCategoryId = bc.BudgetCategoryId,
                                    BudgetCategoryName = bc.BudgetCategoryName,
                                    ExpenseCategoryId = ec != null ? ec.ExpensesCategoryId : (int?)null,
                                    ExpenseCategoryName = ec != null ? ec.ExpensesCategoryName : null,
                                    Description = be.Description,
                                    Amount = be.Amount,
                                    DateUsed = be.DateUsed
                                };

            return await PagedList<BudgetEntryResponseDTO>.ToPagedListAsync(
                budgetEntries.OrderByDescending(be => be.DateUsed).
                ThenByDescending(be => be.BudgetEntryId),
                budgetEntryParameters.PageNumber,
                budgetEntryParameters.PageSize
            );
        }

        public async Task<BudgetEntryResponseDTO> GetBudgetEntryById(int id)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);
            var budgetEntry = from be in _context.BudgetEntries
                                join bc in _context.BudgetCategories on be.BudgetCagetoryId equals bc.BudgetCategoryId
                                join ec in _context.ExpensesCategories on be.ExpenseCategoryId equals ec.ExpensesCategoryId into ecGroup
                                from ec in ecGroup.DefaultIfEmpty()
                                where be.BudgetEntryId == id && be.CreatedBy == _userId 
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
                                };
            var result = await budgetEntry.FirstOrDefaultAsync();

            if (result == null) throw new InvalidOperationException("No budget entry found!");

            return result;
        }

        public async Task<BudgetEntryResponseDTO> AddBudgetEntry(BudgetEntryRequestDTO budgetRequest)
        {
            var newBudgetEntry = new BudgetEntryModel
            {
                BudgetCagetoryId = budgetRequest.BudgetCategoryId,
                ExpenseCategoryId = budgetRequest.ExpenseCategoryId,
                Description = budgetRequest.Description,
                Amount = budgetRequest.Amount,
                DateUsed = budgetRequest.DateUsed,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.AddAsync(newBudgetEntry);
            await _context.SaveChangesAsync();

            return await GetBudgetEntryById(newBudgetEntry.BudgetEntryId);
        }

        public async Task SyncBudgetEntries(List<BudgetEntryRequestDTO> budgetRequestList)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var unbudgetedExpenses = await _context.ExpensesBudget.Where(eb => eb.ExpensesBudgetCategoryId == 3 && eb.CreatedBy == _userId).ToListAsync();

                if(unbudgetedExpenses == null)
                {
                    throw new Exception();
                }

                var newBudgetedExpenses = unbudgetedExpenses.Select(ue => new ExpensesBudgetModel
                {
                    ExpensesBudgetCategoryId = 1,
                    Description = ue.Description,
                    Amount = ue.Amount,
                    CreatedBy= _userId,
                    DateCreated = DateTime.Now
                }).ToList();

                await _context.ExpensesBudget.AddRangeAsync(newBudgetedExpenses);
                await _context.ExpensesBudget
                    .Where(eb => eb.ExpensesBudgetCategoryId == 3 && eb.CreatedBy == _userId)
                    .ExecuteDeleteAsync();

                var entries = budgetRequestList.Select(dto => new BudgetEntryModel
                {
                    BudgetCagetoryId = dto.BudgetCategoryId,
                    ExpenseCategoryId = dto.ExpenseCategoryId,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    DateUsed = dto.DateUsed,
                    CreatedBy = _userId,
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

        public async Task<BudgetEntryResponseDTO> ModifyBudgetEntry(BudgetEntryModifyDTO budgetRequest)
        {
            var budgetEntryToUpdate = await _context.BudgetEntries.FindAsync(budgetRequest.BudgetEntryId);

            if (budgetEntryToUpdate == null) throw new InvalidOperationException("No budget entry found!");

            budgetEntryToUpdate.BudgetCagetoryId = budgetRequest.BudgetCategoryId;
            budgetEntryToUpdate.ExpenseCategoryId = budgetRequest.ExpenseCategoryId;
            budgetEntryToUpdate.Description = budgetRequest.Description;
            budgetEntryToUpdate.Amount = budgetRequest.Amount;
            budgetEntryToUpdate.DateUsed = budgetRequest.DateUsed;
            budgetEntryToUpdate.UpdatedBy = _userId;
            budgetEntryToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetBudgetEntryById(budgetEntryToUpdate.BudgetEntryId);
        }

        public async Task RemoveBudgetEntry(int id)
        {
            var budgetEntryToDelete = await _context.BudgetEntries.FindAsync(id);

            if (budgetEntryToDelete == null) throw new InvalidOperationException("Budget entry not found!");

            _context.BudgetEntries.Remove(budgetEntryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBudgetEntryBulk(List<BudgetEntryDeleteDTO> idList)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var ids = idList.Select(bm => bm.BudgetEntryId).ToList();
                var entriesToDelete = await _context.BudgetEntries
                                            .Where(bm => ids.Contains(bm.BudgetEntryId))
                                            .ToListAsync();

                if (!entriesToDelete.Any()) throw new InvalidOperationException("No budget entries found to delete!");

                _context.BudgetEntries.RemoveRange(entriesToDelete);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Bulk deletion of budget entries failed! Error: ", ex);
            }
        }
    }
}
