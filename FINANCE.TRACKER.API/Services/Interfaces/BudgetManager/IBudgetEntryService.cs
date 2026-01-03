using FINANCE.TRACKER.API.Models.BudgetManager;
using FINANCE.TRACKER.API.Models.DTO.BudgetManager.BudgetEntry;
using FINANCE.TRACKER.API.Repositories;

namespace FINANCE.TRACKER.API.Services.Interfaces.BudgetManager
{
    public interface IBudgetEntryService
    {
        Task<PagedList<BudgetEntryResponseDTO>> GetAllBudgetEntries(BudgetEntryParameters budgetEntryParameters);
        Task<BudgetEntryResponseDTO> GetBudgetEntryById(int id);
        Task SyncBudgetEntries(List<BudgetEntryRequestDTO> budgetRequestList);
        Task<BudgetEntryResponseDTO> AddBudgetEntry(BudgetEntryRequestDTO budgetRequest);
        Task<BudgetEntryResponseDTO> ModifyBudgetEntry(BudgetEntryModifyDTO budgetRequest);
        Task RemoveBudgetEntry(int id);
        Task RemoveBudgetEntryBulk(List<BudgetEntryDeleteDTO> idList);
    }
}
