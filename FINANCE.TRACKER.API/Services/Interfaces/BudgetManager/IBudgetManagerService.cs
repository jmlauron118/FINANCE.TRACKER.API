using FINANCE.TRACKER.API.Models.DTO.BudgetManager;
using Microsoft.AspNetCore.Mvc;

namespace FINANCE.TRACKER.API.Services.Interfaces.BudgetManager
{
    public interface IBudgetManagerService
    {
        Task<IEnumerable<BudgetManagerResponseDTO>> GetAllBudgetEntries();
        Task<BudgetManagerResponseDTO> GetBudgetEntryById(int id);
        Task AddBudgetEntryBulk(List<BudgetManagerRequestDTO> budgetRequestList);
        Task<BudgetManagerResponseDTO> AddBudgetEntry(BudgetManagerRequestDTO budgetRequest);
        Task<BudgetManagerResponseDTO> ModifyBudgetEntry(BudgetManagerModifyDTO budgetRequest);
        Task RemoveBudgetEntry(List<BudgetManagerDeleteDTO> idList);
    }
}
