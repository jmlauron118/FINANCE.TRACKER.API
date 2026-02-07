using System.ComponentModel.DataAnnotations;

namespace FINANCE.TRACKER.API.Models.Savings
{
    public class SavingsTransactionTypeModel
    {
        [Key]
        public int TransactionTypeId { get; set; }

        [Required]
        public string TransactionTypeName { get; set; }

        [Required]
        public string Description { get; set; }

        public int Direction { get; set; }

        public int IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
