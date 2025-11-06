using FINANCE.TRACKER.API.Models.DTO.Category.ExpenseCategoryDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Category
{
    public interface IExpenseCategoryService
    {
        Task<IEnumerable<ExpenseCategoryResponseDTO>> GetAllExpenseCategories(int status);
        Task<ExpenseCategoryResponseDTO> GetExpenseCategoryById(int id);
        Task<ExpenseCategoryResponseDTO> AddExpenseCategory(ExpenseCategoryRequestDTO expenseCategory);
        Task<ExpenseCategoryResponseDTO> ModifyExpenseCategory(ExpenseCategoryModifyDTO expenseCategory);
    }
}
