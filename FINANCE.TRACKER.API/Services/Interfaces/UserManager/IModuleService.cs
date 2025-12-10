using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleResponseDTO>> GetAllModules(int status);

        Task<ModuleResponseDTO> GetModuleById(int id);

        Task<IEnumerable<ModuleResponseDTO>> GetAllParentModules();

        Task<ModuleResponseDTO> AddModule(ModuleRequestDTO module);

        Task<ModuleResponseDTO> ModifyModule(ModuleModifyDTO module);
    }
}
