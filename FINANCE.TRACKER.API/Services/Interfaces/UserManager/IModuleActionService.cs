using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleActionDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IModuleActionService
    {
        Task<IEnumerable<ModuleActionResponseDTO>> GetAllModuleActions();
        Task<ModuleActionResponseDTO> GetModuleActionById(int id);
        Task<ModuleActionResponseDTO> AddModuleAcction(ModuleActionRequestDTO moduleAction);
        Task<ModuleActionResponseDTO> ModifyModuleAction(ModuleActionModifyDTO moduleAction);
        Task RemoveModuleAction(int id);
    }
}
