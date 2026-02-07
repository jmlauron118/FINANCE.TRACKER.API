using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentTypeDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Category
{
    public interface IInvestmentTypeService
    {
        Task<IEnumerable<InvestmentTypeResponseDTO>> GetAllInvestmentTypes(int status);
        Task<InvestmentTypeResponseDTO> GetInvestmentTypeById(int id);
        Task<InvestmentTypeResponseDTO> AddInvestmentType(InvestmentTypeRequestDTO request);
        Task<InvestmentTypeResponseDTO> ModifyInvestmentType(InvestmentTypeModifyDTO request);
    }
}
