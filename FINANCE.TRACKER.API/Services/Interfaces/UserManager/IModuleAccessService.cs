using FINANCE.TRACKER.API.Models.DTO.UserManager.ModuleAccessDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IModuleAccessService
    {
        Task<IEnumerable<ModuleAccessResponseDTO>> GetAllModuleAccess();
        Task<ModuleAccessResponseDTO> GetModuleAccessById(int id);
        Task<ModuleAccessResponseDTO> AddModuleAccess(ModuleAccessRequestDTO moduleAcess);
        Task<ModuleAccessResponseDTO> ModifyModuleAccess(ModuleAccessModifyDTO moduleAccess);
        Task RemoveModuleAccess(int id);
    }
}
