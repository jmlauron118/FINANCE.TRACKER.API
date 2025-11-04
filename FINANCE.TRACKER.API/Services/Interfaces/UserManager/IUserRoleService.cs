using FINANCE.TRACKER.API.Models.DTO.UserManager.UserRoleDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleResponseDTO>> GetAllUserRoles();

        Task<UserRoleResponseDTO> GetUserRoleById(int id);

        Task<UserRoleResponseDTO> AddUserRole(UserRoleRequestDTO userRole);

        Task<UserRoleResponseDTO> ModifyUserRole(UserRoleModifyDTO userRole);

        Task RemoveUserRole(int id);
    }
}
