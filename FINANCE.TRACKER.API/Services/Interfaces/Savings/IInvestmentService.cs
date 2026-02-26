using FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO;
using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Savings
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentResponseDTO>> GetAllInvestments();
        Task ReturnFromInvestment(ReturnFromInvestmentDTO request);
        Task<ReturnTransactionResponseDTO> GetReturnSavingsTransaction(int returnTransactionId);
    }
}
