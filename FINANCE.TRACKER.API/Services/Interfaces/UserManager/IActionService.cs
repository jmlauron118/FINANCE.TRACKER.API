using FINANCE.TRACKER.API.Models.DTO.UserManager.ActionDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.UserManager
{
    public interface IActionService
    {
        Task<IEnumerable<ActionResponseDTO>> GetAllActions(int status = 2);

        Task<ActionResponseDTO> GetActionById(int id);

        Task<ActionResponseDTO> AddAction(ActionRequestDTO action);

        Task<ActionResponseDTO> ModifyAction(ActionModityDTO action);
    }
}
