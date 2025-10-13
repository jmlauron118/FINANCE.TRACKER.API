using FINANCE.TRACKER.API.Models.DTO.UserManager.RoleDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDTO>> GetAllRoles(int status = 2);

        Task<RoleResponseDTO> GetRoleById(int id);

        Task<RoleResponseDTO> AddRole(RoleRequestDTO role);

        Task<RoleResponseDTO> ModifyRole(RoleModifyDTO role);
    }
}
