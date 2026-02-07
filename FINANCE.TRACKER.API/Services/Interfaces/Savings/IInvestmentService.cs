using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Savings
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentResponseDTO>> GetAllInvestments();
    }
}
