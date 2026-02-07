namespace FINANCE.TRACKER.API.Models.DTO.Savings.SavingsTransactionTypeDTO
{
    public class SavingsTransactionTypeRequestDTO
    {
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }
        public int Direction { get; set; }
        public int IsActive { get; set; }
    }
}
