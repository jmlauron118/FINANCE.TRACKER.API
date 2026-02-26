using FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO;

namespace FINANCE.TRACKER.API.Services.Interfaces.Savings
{
    public interface ISavingsService
    {
        Task<IEnumerable<SavingsTransactionResponseDTO>> GetAllSavingsTransactions();
        Task AddSavingsTransaction(SavingsTransactionRequestDTO request);
        Task ModifySavingsTransaction(SavingsTransactionModifyDTO request);
        Task RemoveSavingsTransaction(int transactionId);
        Task<SavingsSummaryResponseDTO> GetSavingsSummary();
    }
}
