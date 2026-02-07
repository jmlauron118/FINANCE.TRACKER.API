using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.Category;
using FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO;
using FINANCE.TRACKER.API.Services.Interfaces.Category;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FINANCE.TRACKER.API.Services.Implementations.Category
{
    public class BudgetCategoryService : IBudgetCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly int _userId;

        public BudgetCategoryService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = int.TryParse(_contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        }

        public async Task<IEnumerable<BudgetCategoryResponseDTO>> GetAllBudgetCategories(int status)
        {
            return await _context.BudgetCategories
                .Where(bc => status == 2 || bc.IsActive == status)
                .Select(bc => new BudgetCategoryResponseDTO
                {
                    BudgetCategoryId = bc.BudgetCategoryId,
                    BudgetCategoryName = bc.BudgetCategoryName,
                    BudgetCategoryDescription = bc.BudgetCategoryDescription,
                    IsActive = bc.IsActive
                }).ToListAsync();
        }

        public async Task<BudgetCategoryResponseDTO> GetBudgetCategoryById(int id)
        {
            var budgetCategory = await _context.BudgetCategories
                .Where(bc => bc.BudgetCategoryId == id)
                .Select(bc => new BudgetCategoryResponseDTO
                {
                    BudgetCategoryId = bc.BudgetCategoryId,
                    BudgetCategoryName = bc.BudgetCategoryName,
                    BudgetCategoryDescription = bc.BudgetCategoryDescription,
                    IsActive = bc.IsActive
                }).FirstOrDefaultAsync();

            if (budgetCategory == null) throw new InvalidOperationException("Budget category not found!");

            return budgetCategory;
        }

        public async Task<BudgetCategoryResponseDTO> AddBudgetCategory(BudgetCategoryRequestDTO budgetCategory)
        {
            var existingBudgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(bc => bc.BudgetCategoryName == budgetCategory.BudgetCategoryName);

            if (existingBudgetCategory != null) throw new InvalidOperationException("Budget category already exist!");

            var newBudgetCategory = new BudgetCategoryModel
            {
                BudgetCategoryName = budgetCategory.BudgetCategoryName,
                BudgetCategoryDescription = budgetCategory.BudgetCategoryDescription,
                IsActive = budgetCategory.IsActive,
                CreatedBy = _userId,
                DateCreated = DateTime.Now
            };

            await _context.BudgetCategories.AddAsync(newBudgetCategory);
            await _context.SaveChangesAsync();

            return await this.GetBudgetCategoryById(newBudgetCategory.BudgetCategoryId);
        }

        public async Task<BudgetCategoryResponseDTO> ModifyBudgetCategory(BudgetCategoryModifyDTO budgetCategory)
        {
            var existingBudgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(bc =>
                    bc.BudgetCategoryName == budgetCategory.BudgetCategoryName &&
                    bc.BudgetCategoryId != budgetCategory.BudgetCategoryId
                );

            if (existingBudgetCategory != null) throw new InvalidOperationException("Budget category already exist!");

            var budgetCategoryToUpdate = await _context.BudgetCategories.FindAsync(budgetCategory.BudgetCategoryId);

            if (budgetCategoryToUpdate == null) throw new InvalidOperationException("Budget category not found!");

            budgetCategoryToUpdate.BudgetCategoryName = budgetCategory.BudgetCategoryName;
            budgetCategoryToUpdate.BudgetCategoryDescription = budgetCategory.BudgetCategoryDescription;
            budgetCategoryToUpdate.IsActive = budgetCategory.IsActive;
            budgetCategoryToUpdate.UpdatedBy = _userId;
            budgetCategoryToUpdate.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            return await this.GetBudgetCategoryById(budgetCategoryToUpdate.BudgetCategoryId);
        }
    }
}
