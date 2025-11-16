using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
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

        public BudgetEntryService(AppDbContext context, IHttpContextAccessor contextAccessor) 
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<PagedList<BudgetEntryResponseDTO>> GetAllBudgetEntries(BudgetEntryParameters budgetEntryParameters)
        {
            var searchValue = budgetEntryParameters.Search?.Trim().ToLower();
            var budgetEntries = from bm in _context.BudgetEntries
                                join bc in _context.BudgetCategories on bm.BudgetCagetoryId equals bc.BudgetCategoryId
                                join ec in _context.ExpensesCategories on bm.ExpenseCategoryId equals ec.ExpensesCategoryId into ecGroup
                                from ec in ecGroup.DefaultIfEmpty()
                                where string.IsNullOrEmpty(searchValue) || (bm.Description != null && bm.Description.ToLower().Contains(searchValue))
                                orderby bm.DateUsed descending
                                select new BudgetEntryResponseDTO
                                {
                                    BudgetEntryId = bm.BudgetEntryId,
                                    BudgetCategoryId = bc.BudgetCategoryId,
                                    BudgetCategoryName = bc.BudgetCategoryName,
                                    ExpenseCategoryId = ec != null ? ec.ExpensesCategoryId : (int?)null,
                                    ExpenseCategoryName = ec != null ? ec.ExpensesCategoryName : null,
                                    Description = bm.Description,
                                    Amount = bm.Amount,
                                    DateUsed = bm.DateUsed
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
            var budgetEntry = from bm in _context.BudgetEntries
                                join bc in _context.BudgetCategories on bm.BudgetCagetoryId equals bc.BudgetCategoryId
                                join ec in _context.ExpensesCategories on bm.ExpenseCategoryId equals ec.ExpensesCategoryId into ecGroup
                                from ec in ecGroup.DefaultIfEmpty()
                                where bm.BudgetEntryId == id
                                orderby bm.DateUsed
                                select new BudgetEntryResponseDTO
                                {
                                    BudgetEntryId = bm.BudgetEntryId,
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

        public async Task<BudgetEntryResponseDTO> AddBudgetEntry(BudgetEntryRequestDTO budgetRequest)
        {
            var newBudgetEntry = new BudgetEntryModel
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

            return await GetBudgetEntryById(newBudgetEntry.BudgetEntryId);
        }

        public async Task AddBudgetEntryBulk(List<BudgetEntryRequestDTO> budgetRequestList)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var entries = budgetRequestList.Select(dto => new BudgetEntryModel
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

        public async Task<BudgetEntryResponseDTO> ModifyBudgetEntry(BudgetEntryModifyDTO budgetRequest)
        {
            var budgetEntryToUpdate = await _context.BudgetEntries.FindAsync(budgetRequest.BudgetEntryId);

            if (budgetEntryToUpdate == null) throw new InvalidOperationException("No budget entry found!");

            budgetEntryToUpdate.BudgetCagetoryId = budgetRequest.BudgetCategoryId;
            budgetEntryToUpdate.ExpenseCategoryId = budgetRequest.ExpenseCategoryId;
            budgetEntryToUpdate.Description = budgetRequest.Description;
            budgetEntryToUpdate.Amount = budgetRequest.Amount;
            budgetEntryToUpdate.DateUsed = budgetRequest.DateUsed;
            budgetEntryToUpdate.UpdatedBy = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
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

                if (entriesToDelete.Any())
                {
                    _context.BudgetEntries.RemoveRange(entriesToDelete);
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
