using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.Savings
{
    public class InvestmentModel
    {
        [Key]
        public int InvestmentId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int InvestmentTypeId { get; set; }

        public int? ReturnTransactionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RelializedAmount { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
