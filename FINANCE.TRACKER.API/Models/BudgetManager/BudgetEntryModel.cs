using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.BudgetManager
{
    public class BudgetEntryModel
    {
        [Key]
        public int BudgetEntryId { get; set; }

        public int BudgetCagetoryId { get; set; }

        public int? ExpenseCategoryId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public DateTime DateUsed { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
