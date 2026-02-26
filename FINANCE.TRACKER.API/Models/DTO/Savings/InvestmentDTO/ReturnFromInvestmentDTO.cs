namespace FINANCE.TRACKER.API.Models.DTO.Savings.InvestmentDTO
{
    public class ReturnFromInvestmentDTO
    {
        public int InvestmentId { get; set; }
        public string ReturnDescription { get; set; }
        public decimal AmountReturned { get; set; }
        public DateTime DateReturned { get; set; }
    }
}
