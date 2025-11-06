using FINANCE.TRACKER.API.Models.DTO.UserManager.UserDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsers(int status = 2);
        Task<UserResponseDTO> GetUserById(int id);
        Task<UserResponseDTO> AddUser(UserRequestDTO user);
        Task<UserResponseDTO> ModifyUser(UserModifyDTO user);
        Task<IEnumerable<UserModuleResponseDTO>> GetUserModules(int userId);
    }
}
