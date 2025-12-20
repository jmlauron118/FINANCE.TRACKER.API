using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FINANCE.TRACKER.API.Models.BudgetManager
{
    public class ExpensesBudgetModel
    {
        [Key]
        public int Id { get; set; }

        public int ExpensesBudgetCategoryId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Column(TypeName = "nvarchar(150)")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
