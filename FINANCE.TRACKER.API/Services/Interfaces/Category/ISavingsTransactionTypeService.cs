using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionTypeDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Category
{
    public interface ISavingsTransactionTypeService
    {
        Task<IEnumerable<SavingsTransactionTypeResponseDTO>> GetAllSavingsTransactionTypes(int status, int type);
        Task<SavingsTransactionTypeResponseDTO> GetSavingsTransactionTypeById(int id);
        Task<SavingsTransactionTypeResponseDTO> AddSavingsTransactionType(SavingsTransactionTypeRequestDTO request);
        Task<SavingsTransactionTypeResponseDTO> ModifySavingsTransactionType(SavingsTransactionTypeModifyDTO request);
    }
}
