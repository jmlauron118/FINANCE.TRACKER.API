namespace FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionDTO
{
    public class SavingsTransactionResponseDTO: SavingsTransactionRequestDTO
    {
        public int TransactionId { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
