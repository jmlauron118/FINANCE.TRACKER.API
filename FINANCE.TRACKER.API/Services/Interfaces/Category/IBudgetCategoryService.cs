using FINANCE.TRACKER.API.Models.DTO.Category.BudgetCategoryDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Category
{
    public interface IBudgetCategoryService
    {
        Task<IEnumerable<BudgetCategoryResponseDTO>> GetAllBudgetCategories(int status);
        Task<BudgetCategoryResponseDTO> GetBudgetCategoryById(int id);
        Task<BudgetCategoryResponseDTO> AddBudgetCategory(BudgetCategoryRequestDTO budgetCategory);
        Task<BudgetCategoryResponseDTO> ModifyBudgetCategory(BudgetCategoryModifyDTO budgetCategory);
    }
}
