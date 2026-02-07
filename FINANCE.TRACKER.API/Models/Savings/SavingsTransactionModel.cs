using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.Savings
{
    public class SavingsTransactionModel
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DateUsed { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
